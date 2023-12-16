using SocketCore.Utils;
using SocketServer.Mock;
using Process = SocketDto.Process;

namespace SocketServer.SocketHeper;

internal class TcpHelper : ISocketBase
{
    public static long SystemId { get; private set; } // 服务端标识，TCP数据接收时保存，用于UDP数据包识别
    private static bool _isRunSuccess;
    private static readonly Socket Server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static readonly ConcurrentDictionary<string, Socket> Clients = new();
    private static readonly ConcurrentDictionary<string, ConcurrentQueue<INetObject>> ReceivedCommands = new();
    private ConcurrentQueue<INetObject> NeedSendCommands { get; } = new();

    #region 公开接口

    public void Start(string ip, int port)
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    Server.Bind(ipEndPoint);
                    Server.Listen(10);
                    _isRunSuccess = true;

                    ReceiveCommand();
                    StartDillReceivedCommand();
                    SendCommands();

                    Logger.Info($"Tcp服务启动成功：{ipEndPoint}，等待客户端连接");
                    break;
                }
                catch (Exception ex)
                {
                    _isRunSuccess = false;
                    Logger.Warning($"运行TCP服务异常，1秒后将重新运行：{ex.Message}");
                    Thread.Sleep(1);
                }
            }
        });
    }

    public void Stop()
    {
        _isRunSuccess = false;
        try
        {
            Server.Close(0);
            Logger.Info($"停止Tcp服务");
        }
        catch (Exception ex)
        {
            Logger.Warning($"停止TCP服务异常：{ex.Message}");
        }
    }

    public void SendCommand(INetObject command)
    {
        NeedSendCommands.Enqueue(command);
    }

    public bool TryGetResponse(out INetObject? response)
    {
        throw new NotImplementedException();
    }

    private static int _taskId = 0;

    public static int GetNewTaskId()
    {
        return ++_taskId;
    }

    #endregion

    #region 私有方法

    private static void ReceiveCommand()
    {
        Task.Run(() =>
        {
            while (_isRunSuccess)
            {
                try
                {
                    var socketClient = Server.Accept();

                    var socketClientKey = $"{socketClient.RemoteEndPoint}";
                    Clients[socketClientKey] = socketClient;

                    Logger.Info($"客户端({socketClientKey})连接上线");

                    ReceiveCommand(socketClient);
                }
                catch (Exception ex)
                {
                    Logger.Error($"处理客户端连接上线异常：{ex.Message}");
                }
            }
        });
    }

    private static void ReceiveCommand(Socket tcpClient)
    {
        Task.Run(() =>
        {
            byte[]? receivedBuffer = null;
            while (_isRunSuccess)
            {
                try
                {
                    // 1、接收数据包
                    var buffer = new byte[500 * 1024];
                    var bytesReadLen = tcpClient.Receive(buffer);
                    if (bytesReadLen <= 0)
                    {
                        continue;
                    }

                    var newBufferLen = (receivedBuffer?.Length ?? 0) + bytesReadLen;
                    var newAllBuffer = new byte[newBufferLen];
                    var addBufferIndex = 0;
                    if (receivedBuffer != null)
                    {
                        Buffer.BlockCopy(receivedBuffer, 0, newAllBuffer, 0, receivedBuffer.Length);
                        addBufferIndex = receivedBuffer.Length;
                    }

                    Buffer.BlockCopy(buffer, 0, newAllBuffer, addBufferIndex, bytesReadLen);
                    receivedBuffer = newAllBuffer;

                    // 2、解析数据包
                    if (receivedBuffer.Length >= SerializeHelper.PacketHeadLen)
                    {
                        receivedBuffer = ReceiveCommand(tcpClient, receivedBuffer);
                    }

                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                }
                catch (SocketException ex)
                {
                    Logger.Error($"远程主机异常，将移除该客户端：{ex.Message}");
                    RemoveClient(tcpClient);
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
        });
    }

    private static byte[]? ReceiveCommand(Socket tcpClient, byte[] receivedBuffer)
    {
        while (true)
        {
            var readIndex = 0;
            if (!SerializeHelper.ReadHead(receivedBuffer, ref readIndex, out var netObject))
            {
                return receivedBuffer;
            }

            SystemId = netObject!.SystemId;

            // 读取到完整数据包后才解析数据
            if (receivedBuffer.Length < netObject!.BufferLen)
            {
                return receivedBuffer;
            }

            byte[]? buffer;
            if (receivedBuffer.Length == netObject.BufferLen)
            {
                buffer = receivedBuffer;
            }
            else
            {
                buffer = new byte[netObject.BufferLen];
                Buffer.BlockCopy(receivedBuffer, 0, buffer, 0, netObject.BufferLen);
            }

            // 解析对象
            ReceiveCommand(tcpClient, buffer, netObject);

            // 除去已经解析的包
            if (receivedBuffer.Length == netObject.BufferLen)
            {
                return default;
            }

            var newPaketLen = (int)(receivedBuffer.Length - netObject.BufferLen);
            var newPacket = new byte[newPaketLen];

            Buffer.BlockCopy(receivedBuffer, (int)netObject.BufferLen, newPacket, 0, newPaketLen);
            return newPacket;
        }
    }

    private static void ReceiveCommand(Socket tcpClient, byte[] buffer, NetObjectHeadInfo netObjectHeadInfo)
    {
        var sw = Stopwatch.StartNew();

        INetObject command;

        if (netObjectHeadInfo.IsNetObject<RequestBaseInfo>())
        {
            command = SerializeHelper.Deserialize<RequestBaseInfo>(buffer);
        }
        else if (netObjectHeadInfo.IsNetObject<RequestProcess>())
        {
            command = SerializeHelper.Deserialize<RequestProcess>(buffer);
        }
        else if (netObjectHeadInfo.IsNetObject<Heartbeat>())
        {
            command = SerializeHelper.Deserialize<Heartbeat>(buffer);
        }
        else
        {
            throw new Exception(
                $"非法数据包：{netObjectHeadInfo}");
        }

        sw.Stop();

        var tcpClientKey = tcpClient.RemoteEndPoint!.ToString()!;
        if (!ReceivedCommands.ContainsKey(tcpClientKey))
        {
            ReceivedCommands[tcpClientKey] = new ConcurrentQueue<INetObject>();
        }

        ReceivedCommands[tcpClientKey].Enqueue(command);

        Logger.Info($"解析数据包{command.GetType().Name}({netObjectHeadInfo.ObjectId})用时{sw.ElapsedMilliseconds} ms");
    }

    private static void RemoveClient(Socket tcpClient)
    {
        var key = tcpClient.RemoteEndPoint!.ToString()!;
        Clients.TryRemove(key, out _);
        ReceivedCommands.TryRemove(key, out _);
    }

    private static void StartDillReceivedCommand()
    {
        Task.Run(() =>
        {
            while (true)
            {
                DillReceivedCommand();
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }

    private static void DillReceivedCommand()
    {
        var needRemoveKeys = new List<string>();
        foreach (var clientCommands in ReceivedCommands)
        {
            var clientKey = clientCommands.Key;
            if (Clients.TryGetValue(clientKey, out var client))
            {
                var isDisconnected = client.Poll(1, SelectMode.SelectRead) && client.Available == 0;
                if (isDisconnected)
                {
                    needRemoveKeys.Add(clientKey);
                    continue;
                }

                while (clientCommands.Value.TryDequeue(out var command))
                {
                    DillReceivedCommand(client, command);
                }
            }
            else
            {
                needRemoveKeys.Add(clientKey);
            }
        }

        if (needRemoveKeys.Count > 0)
        {
            needRemoveKeys.ForEach(key => ReceivedCommands.TryRemove(key, out _));
        }
    }

    private static void DillReceivedCommand(Socket tcpClient, INetObject command)
    {
        switch (command)
        {
            case RequestBaseInfo requestBaseInfo:
                DillReceivedCommand(tcpClient, requestBaseInfo);
                break;
            case RequestProcess requestProcess:
                DillReceivedCommand(tcpClient, requestProcess);
                break;
            case Heartbeat _:
            {
                DillReceivedCommand(tcpClient);
                break;
            }
            default:
                throw new Exception($"未处理命令{command.GetType().Name}");
        }

        Thread.Sleep(TimeSpan.FromMilliseconds(1));

        Logger.Info($"处理命令{command.GetType().Name}");
    }

    private static void DillReceivedCommand(Socket tcpClient, RequestBaseInfo command)
    {
        var response = new ResponseBaseInfo()
        {
            TaskId = command.TaskId,
            OperatingSystemType = "Windows 11",
            MemorySize = 48 * 1024,
            ProcessorCount = 8,
            TotalDiskSpace = 1024,
            NetworkBandwidth = 1024,
            IPAddress = "192.32.35.23",
            ServerName = "Windows server 2021",
            DataCenterLocation = "成都",
            IsRunning = true,
            LastUpdateTime = TimestampHelper.GetTimestamp()
        };
        tcpClient.Send(response.Serialize(SystemId));
    }

    private static void DillReceivedCommand(Socket tcpClient, RequestProcess command)
    {
        var pageCount = MockUtil.GetPageCount(MockUtil.MockCount, MockUtil.MockPageSize);
        for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var response = new ResponseProcess()
            {
                Processes = MockUtil.MockProcesses(pageIndex)
            };
            tcpClient.Send(response.Serialize(SystemId));
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
        }
    }

    private static void DillReceivedCommand(Socket tcpClient)
    {
        tcpClient.Send(new Heartbeat().Serialize(SystemId));
    }

    private void SendCommands()
    {
        Task.Run(() =>
        {
            while (_isRunSuccess)
            {
                if (NeedSendCommands.TryDequeue(out var command))
                {
                    try
                    {
                        foreach (var client in Clients)
                        {
                            client.Value.Send(command.Serialize(SystemId));
                        }
                    }
                    catch (Exception ex)
                    {
                        NeedSendCommands.Enqueue(command);
                        Logger.Error($"发送命令{command.GetType().Name}失败，将排队重新发送: {ex.Message}");
                    }
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(5));
            }
        });
    }

    #endregion
}