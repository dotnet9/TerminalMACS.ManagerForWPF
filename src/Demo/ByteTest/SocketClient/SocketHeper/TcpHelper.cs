namespace SocketClient.SocketHeper;

internal class TcpHelper : ISocketBase
{
    public static long SystemId { get; private set; } // 服务端标识，TCP数据接收时保存，用于UDP数据包识别
    private static readonly Socket Client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    /// <summary>
    /// 客户端连接对象，键为客户端连接地址
    /// </summary>
    private static readonly ConcurrentDictionary<string, Socket> Clients = new();

    /// <summary>
    /// 接收命令列表
    /// </summary>
    private static readonly BlockingCollection<INetObject> ReceivedCommands = new();

    /// <summary>
    /// 需要发送的命令
    /// </summary>
    private ConcurrentQueue<INetObject> NeedSendCommands { get; } = new();

    #region 公开接口

    public static bool IsRunning { get; private set; }

    public void Start(string ip, int port)
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    Client.Connect(ipEndPoint);
                    IsRunning = true;

                    ReceiveCommand();
                    SendCommands();

                    Logger.Info($"连接Tcp服务成功");
                    break;
                }
                catch (Exception ex)
                {
                    IsRunning = false;
                    Logger.Warning($"连接TCP服务异常，1秒后将重新连接：{ex.Message}");
                    Thread.Sleep(1);
                }
            }
        });
    }

    public void Stop()
    {
        IsRunning = false;
        try
        {
            Client.Close(0);
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
        return ReceivedCommands.TryTake(out response);
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
            byte[]? receivedBuffer = null;
            while (IsRunning)
            {
                try
                {
                    // 1、接收数据包
                    var buffer = new byte[500 * 1024];
                    var bytesReadLen = Client.Receive(buffer);
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
                        receivedBuffer = ReceiveCommand(receivedBuffer);
                    }

                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                }
                catch (SocketException ex)
                {
                    Logger.Error($"接收数据异常：{ex.Message}");
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
        });
    }

    private static byte[]? ReceiveCommand(byte[] receivedBuffer)
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
            ReceiveCommand(buffer, netObject);

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

    private static void ReceiveCommand(byte[] buffer, NetObjectHeadInfo netObjectHeadInfo)
    {
        var sw = Stopwatch.StartNew();

        INetObject command;

        if (netObjectHeadInfo.IsNetObject<ResponseBaseInfo>())
        {
            command = SerializeHelper.Deserialize<ResponseBaseInfo>(buffer);
        }
        else if (netObjectHeadInfo.IsNetObject<ResponseProcess>())
        {
            command = SerializeHelper.Deserialize<ResponseProcess>(buffer);
        }
        else if (netObjectHeadInfo.IsNetObject<UpdateProcess>())
        {
            command = SerializeHelper.Deserialize<UpdateProcess>(buffer);
        }
        else if (netObjectHeadInfo.IsNetObject<UpdateActiveProcess>())
        {
            command = SerializeHelper.Deserialize<UpdateActiveProcess>(buffer);
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

        ReceivedCommands.Add(command);

        Logger.Info($"解析数据包{command.GetType().Name}({netObjectHeadInfo.ObjectId})用时{sw.ElapsedMilliseconds} ms");
    }

    private void SendCommands()
    {
        Task.Run(() =>
        {
            while (IsRunning)
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