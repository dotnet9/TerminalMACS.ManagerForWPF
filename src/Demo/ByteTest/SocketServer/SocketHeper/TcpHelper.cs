using SocketServer.Mock;
using System.Net.Sockets;

namespace SocketServer.SocketHeper;

public class TcpHelper : BindableBase, ISocketBase
{
    public long SystemId { get; private set; } // 服务端标识，TCP数据接收时保存，用于UDP数据包识别


    private Socket? _server;
    private readonly ConcurrentDictionary<string, Socket> _clients = new();
    private readonly ConcurrentDictionary<string, ConcurrentQueue<INetObject>> _receivedCommands = new();
    private BlockingCollection<INetObject> NeedSendCommands { get; } = new();

    #region 公开接口

    private string _ip = "127.0.0.1";

    /// <summary>
    /// Tcp服务IP
    /// </summary>
    public string Ip
    {
        get => _ip;
        set => SetProperty(ref _ip, value);
    }

    private int _port = 5000;

    /// <summary>
    /// Tcp服务端口
    /// </summary>
    public int Port
    {
        get => _port;
        set => SetProperty(ref _port, value);
    }

    private bool _isStarted;

    /// <summary>
    /// 是否开启Tcp服务
    /// </summary>
    public bool IsStarted
    {
        get => _isStarted;
        set
        {
            if (value != _isStarted)
            {
                SetProperty(ref _isStarted, value);
            }
        }
    }

    private bool _isRunning;

    /// <summary>
    /// 是否正在运行Tcp服务
    /// </summary>
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (value != _isRunning)
            {
                SetProperty(ref _isRunning, value);
            }
        }
    }

    private DateTime _sendTime;

    /// <summary>
    /// 命令发送时间
    /// </summary>
    public DateTime SendTime
    {
        get => _sendTime;
        set
        {
            if (value != _sendTime)
            {
                SetProperty(ref _sendTime, value);
            }
        }
    }

    private DateTime _receiveTime;

    /// <summary>
    /// 响应接收时间
    /// </summary>
    public DateTime ReceiveTime
    {
        get => _receiveTime;
        set
        {
            if (value != _receiveTime)
            {
                SetProperty(ref _receiveTime, value);
            }
        }
    }

    private DateTime _heartbeatTime;

    public DateTime HeartbeatTime
    {
        get => _heartbeatTime;
        set => SetProperty(ref _heartbeatTime, value);
    }

    private int _mockCount = 1000000;

    /// <summary>
    /// 模拟数据总量
    /// </summary>
    public int MockCount
    {
        get => _mockCount;
        set => SetProperty(ref _mockCount, value);
    }

    private int _mockPageSize = 5000;

    /// <summary>
    /// 模拟分包数据量
    /// </summary>
    public int MockPageSize
    {
        get => _mockPageSize;
        set => SetProperty(ref _mockPageSize, value);
    }

    public void Start()
    {
        if (IsStarted)
        {
            Logger.Warning("Tcp服务已经开启");
            return;
        }

        IsStarted = true;

        var ipEndPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);
        Task.Run(() =>
        {
            while (IsStarted)
            {
                try
                {
                    _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _server.Bind(ipEndPoint);
                    _server.Listen(10);
                    IsRunning = true;

                    ReceiveCommand();
                    StartDillReceivedCommand();
                    SendCommands();
                    MockUpdate();

                    Logger.Info($"Tcp服务启动成功：{ipEndPoint}，等待客户端连接");
                    break;
                }
                catch (Exception ex)
                {
                    IsRunning = false;
                    Logger.Warning($"运行TCP服务异常，3秒后将重新运行：{ex.Message}");
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }
        });
    }

    public void Stop()
    {
        if (!IsStarted)
        {
            Logger.Warning("Tcp服务已经关闭");
            return;
        }

        IsStarted = false;

        try
        {
            _server?.Close(0);
            _server = null;
            Logger.Info($"停止Tcp服务");
        }
        catch (Exception ex)
        {
            Logger.Warning($"停止TCP服务异常：{ex.Message}");
        }

        IsRunning = false;
    }

    public void SendCommand(INetObject command)
    {
        if (!IsRunning)
        {
            Logger.Error("Tcp服务未运行，无法发送命令");
            return;
        }

        NeedSendCommands.Add(command);
        Logger.Info($"已将命令{command.GetType()}压入队列，请等待命令发送");
    }

    public void SendCommandBuffer(byte[] buffer)
    {
        throw new NotImplementedException();
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

    private void ReceiveCommand()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                try
                {
                    var socketClient = _server!.Accept();

                    var socketClientKey = $"{socketClient.RemoteEndPoint}";
                    _clients[socketClientKey] = socketClient;

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

    private void ReceiveCommand(Socket tcpClient)
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
                    var bytesReadLen = tcpClient.Receive(buffer);
                    ReceiveTime = DateTime.Now;
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

    private byte[]? ReceiveCommand(Socket tcpClient, byte[] receivedBuffer)
    {
        while (IsRunning)
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

        return default;
    }

    private void ReceiveCommand(Socket tcpClient, byte[] buffer, NetObjectHeadInfo netObjectHeadInfo)
    {
        var sw = Stopwatch.StartNew();

        INetObject command;

        if (netObjectHeadInfo.IsNetObject<RequestBaseInfo>())
        {
            command = buffer.Deserialize<RequestBaseInfo>();
        }
        else if (netObjectHeadInfo.IsNetObject<RequestProcess>())
        {
            command = buffer.Deserialize<RequestProcess>();
        }
        else if (netObjectHeadInfo.IsNetObject<Heartbeat>())
        {
            command = buffer.Deserialize<Heartbeat>();
        }
        else
        {
            throw new Exception(
                $"非法数据包：{netObjectHeadInfo}");
        }

        sw.Stop();

        var tcpClientKey = tcpClient.RemoteEndPoint!.ToString()!;
        if (!_receivedCommands.TryGetValue(tcpClientKey, out ConcurrentQueue<INetObject>? value))
        {
            value = new ConcurrentQueue<INetObject>();
            _receivedCommands[tcpClientKey] = value;
        }

        value.Enqueue(command);

        Logger.Info($"解析数据包{command.GetType().Name}({netObjectHeadInfo.ObjectId})用时{sw.ElapsedMilliseconds} ms");
    }

    private void RemoveClient(Socket tcpClient)
    {
        var key = tcpClient.RemoteEndPoint!.ToString()!;
        _clients.TryRemove(key, out _);
        _receivedCommands.TryRemove(key, out _);
        Logger.Warning($"已清除客户端信息{key}");
    }

    private void RemoveClient(string key)
    {
        _clients.TryRemove(key, out _);
        _receivedCommands.TryRemove(key, out _);
        Logger.Warning($"已清除客户端信息{key}");
    }

    private void StartDillReceivedCommand()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                DillReceivedCommand();
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }

    private void DillReceivedCommand()
    {
        var needRemoveKeys = new List<string>();
        foreach (var clientCommands in _receivedCommands)
        {
            var clientKey = clientCommands.Key;
            if (_clients.TryGetValue(clientKey, out var client))
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
            needRemoveKeys.ForEach(RemoveClient);
        }
    }

    private void DillReceivedCommand(Socket tcpClient, INetObject command)
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

    private void DillReceivedCommand(Socket tcpClient, RequestBaseInfo command)
    {
        var response = MockUtil.MockBase(command.TaskId);
        tcpClient.Send(response.Serialize(SystemId));
        Logger.Info($"发送{response.GetType()}响应");
    }

    private void DillReceivedCommand(Socket tcpClient, RequestProcess command)
    {
        var pageCount = MockUtil.GetPageCount(MockCount, MockPageSize);
        var sendCount = 0;
        for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var response = new ResponseProcess()
            {
                TaskId = command.TaskId,
                TotalSize = MockCount,
                PageSize = MockPageSize,
                PageCount = pageCount,
                PageIndex = pageIndex,
                Processes = MockUtil.MockProcesses(MockCount, MockPageSize, pageIndex)
            };
            sendCount += response.Processes.Count;
            tcpClient.Send(response.Serialize(SystemId));

            var msg = response.TaskId == default ? $"推送" : "响应请求";
            Logger.Info(
                $"{msg}【{response.PageIndex + 1}/{response.PageCount}】进程{response.Processes.Count}条({sendCount}/{response.TotalSize})");

            Thread.Sleep(TimeSpan.FromMilliseconds(1));
        }
    }

    private void DillReceivedCommand(Socket tcpClient)
    {
        tcpClient.Send(new Heartbeat().Serialize(SystemId));
        HeartbeatTime = DateTime.Now;
    }

    private void SendCommands()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                if (!NeedSendCommands.TryTake(out var command, TimeSpan.FromMilliseconds(1)) || !IsRunning)
                {
                    continue;
                }

                var needRemoveKeys = new List<string>();
                try
                {
                    foreach (var client in _clients)
                    {
                        var isDisconnected = client.Value.Poll(1, SelectMode.SelectRead);
                        if (isDisconnected)
                        {
                            needRemoveKeys.Add(client.Key);
                            continue;
                        }

                        client.Value.Send(command.Serialize(SystemId));
                        SendTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    NeedSendCommands.Add(command);
                    Logger.Error($"发送命令{command.GetType().Name}失败，将排队重新发送: {ex.Message}");
                }

                if (needRemoveKeys.Count > 0)
                {
                    needRemoveKeys.ForEach(RemoveClient);
                }
            }
        });
    }

    private void MockUpdate()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                if (_clients?.Count > 0)
                {
                    UpdateData();
                }

                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
        });
    }

    public void UpdateData()
    {
        var updatePoints = new UpdateProcess()
        {
            Processes = MockUtil.MockProcesses(MockCount, MockPageSize)
        };
        SendCommand(updatePoints);
    }

    #endregion
}