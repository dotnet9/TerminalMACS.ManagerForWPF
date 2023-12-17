namespace SocketClient.SocketHeper;

public class TcpHelper : BindableBase, ISocketBase
{
    public long SystemId { get; private set; } // 服务端标识，TCP数据接收时保存，用于UDP数据包识别
    private Socket? _client;

    /// <summary>
    /// 接收命令列表
    /// </summary>
    private readonly BlockingCollection<INetObject> _receivedCommands = new();

    /// <summary>
    /// 需要发送的命令
    /// </summary>
    private ConcurrentQueue<INetObject> NeedSendCommands { get; } = new();

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

    public void Start()
    {
        if (IsStarted)
        {
            Logger.Warning("Tcp连接已经开启");
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
                    _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _client.Connect(ipEndPoint);
                    IsRunning = true;

                    ReceiveCommand();
                    SendCommands();

                    Logger.Info($"连接Tcp服务成功");
                    break;
                }
                catch (Exception ex)
                {
                    IsRunning = false;
                    Logger.Warning($"连接TCP服务异常，3秒后将重新连接：{ex.Message}");
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }
        });
    }

    public void Stop()
    {
        if (!IsStarted)
        {
            Logger.Warning("Tcp连接已经关闭");
            return;
        }

        IsStarted = false;

        try
        {
            _client?.Close(0);
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

        NeedSendCommands.Enqueue(command);
        Logger.Info($"已将命令{command.GetType()}压入队列，请等待命令发送");
    }

    public bool TryGetResponse(out INetObject? response)
    {
        return _receivedCommands.TryTake(out response);
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
            byte[]? receivedBuffer = null;
            while (IsRunning)
            {
                try
                {
                    // 1、接收数据包
                    var buffer = new byte[500 * 1024];
                    var bytesReadLen = _client!.Receive(buffer);
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

    private byte[]? ReceiveCommand(byte[] receivedBuffer)
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

        return default;
    }

    private void ReceiveCommand(byte[] buffer, NetObjectHeadInfo netObjectHeadInfo)
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

        _receivedCommands.Add(command);

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
                        _client?.Send(command.Serialize(SystemId));
                        SendTime = DateTime.Now;
                        Logger.Info($"已将命令{command.GetType()}发送");
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