using SocketServer.Mock;

namespace SocketServer.SocketHeper;

public class TcpHelper : BindableBase, ISocketBase
{
    public long SystemId { get; private set; } // 服务端标识，TCP数据接收时保存，用于UDP数据包识别

    private Socket? _server;
    private readonly ConcurrentDictionary<string, Socket> _clients = new();
    private readonly ConcurrentDictionary<string, ConcurrentQueue<INetObject>> _requests = new();

    #region 公开属性

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

    #endregion

    #region 公开接口方法

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

                    ListenForClients();
                    ProcessingRequests();
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
        if (_clients.IsEmpty)
        {
            Logger.Error("没有客户端上线，无发送目的地址，无法发送命令");
            return;
        }

        var buffer = command.Serialize(SystemId);
        foreach (var client in _clients)
        {
            client.Value.Send(buffer);
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
        }

        Logger.Info($"发送命令{command.GetType()}");
    }

    public void SendCommand(Socket client, INetObject command)
    {
        var buffer = command.Serialize(SystemId);
        client.Send(buffer);

        Logger.Info($"发送命令{command.GetType()}");
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

    #region 接收客户端命令

    private void RemoveClient(Socket tcpClient)
    {
        RemoveClient(tcpClient.RemoteEndPoint!.ToString()!);
    }

    private void RemoveClient(string key)
    {
        _clients.TryRemove(key, out _);
        _requests.TryRemove(key, out _);
        Logger.Warning($"已清除客户端信息{key}");
    }

    private void ListenForClients()
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

                    HandleClient(socketClient);
                }
                catch (Exception ex)
                {
                    Logger.Error($"处理客户端连接上线异常：{ex.Message}");
                }
            }
        });
    }

    private void HandleClient(Socket tcpClient)
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                try
                {
                    while (tcpClient.ReadPacket(out var buffer, out var objectInfo))
                    {
                        ReadCommand(tcpClient, buffer, objectInfo);
                    }
                }
                catch (SocketException ex)
                {
                    Logger.Error($"远程主机异常，将移除该客户端：{ex.Message}");
                    RemoveClient(tcpClient);
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error($"接收数据异常：{ex.Message}");
                }
            }
        });
    }

    private void ReadCommand(Socket tcpClient, byte[] buffer, NetObjectHeadInfo netObjectHeadInfo)
    {
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

        var tcpClientKey = tcpClient.RemoteEndPoint!.ToString()!;
        if (!_requests.TryGetValue(tcpClientKey, out ConcurrentQueue<INetObject>? value))
        {
            value = new ConcurrentQueue<INetObject>();
            _requests[tcpClientKey] = value;
        }

        value.Enqueue(command);
    }

    #endregion

    #region 处理客户端请求

    private void ProcessingRequests()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                var needRemoveKeys = new List<string>();
                foreach (var request in _requests)
                {
                    var clientKey = request.Key;
                    if (!_clients.TryGetValue(clientKey, out var client))
                    {
                        needRemoveKeys.Add(clientKey);
                        continue;
                    }

                    if (client.Poll(1, SelectMode.SelectRead) && client.Available == 0)
                    {
                        needRemoveKeys.Add(clientKey);
                        continue;
                    }

                    while (request.Value.TryDequeue(out var command))
                    {
                        ProcessingRequest(client, command);
                    }
                }

                if (needRemoveKeys.Count > 0)
                {
                    needRemoveKeys.ForEach(RemoveClient);
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }
        });
    }

    private void ProcessingRequest(Socket tcpClient, INetObject request)
    {
        switch (request)
        {
            case RequestBaseInfo requestBaseInfo:
                ProcessingRequest(tcpClient, requestBaseInfo);
                break;
            case RequestProcess requestProcess:
                ProcessingRequest(tcpClient, requestProcess);
                break;
            case Heartbeat _:
                ProcessingRequest(tcpClient);
                break;
            default:
                throw new Exception($"未处理命令{request.GetType().Name}");
        }
    }

    private void ProcessingRequest(Socket client, RequestBaseInfo request)
    {
        SendCommand(client, MockUtil.MockBase(request.TaskId));
    }

    private void ProcessingRequest(Socket client, RequestProcess request)
    {
        var pageCount = MockUtil.GetPageCount(MockCount, MockPageSize);
        var sendCount = 0;
        for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var response = new ResponseProcess()
            {
                TaskId = request.TaskId,
                TotalSize = MockCount,
                PageSize = MockPageSize,
                PageCount = pageCount,
                PageIndex = pageIndex,
                Processes = MockUtil.MockProcesses(MockCount, MockPageSize, pageIndex)
            };
            sendCount += response.Processes.Count;
            SendCommand(client, response);

            var msg = response.TaskId == default ? $"推送" : "响应请求";
            Logger.Info(
                $"{msg}【{response.PageIndex + 1}/{response.PageCount}】进程{response.Processes.Count}条({sendCount}/{response.TotalSize})");

            Thread.Sleep(TimeSpan.FromMilliseconds(1));
        }
    }

    private void ProcessingRequest(Socket client)
    {
        SendCommand(client, new Heartbeat());
        HeartbeatTime = DateTime.Now;
    }

    #endregion

    #region 更新数据

    private void MockUpdate()
    {
        Task.Run(() =>
        {
            var isUpdateAll = false;
            while (IsRunning)
            {
                UpdateAllData(isUpdateAll);
                isUpdateAll = !isUpdateAll;

                Thread.Sleep(TimeSpan.FromMinutes(4));
            }
        });
    }

    public void UpdateAllData(bool isUpdateAll)
    {
        if (isUpdateAll)
        {
            SendCommand(new ChangeProcess());
            Logger.Info("====TCP推送结构变化通知====");
            return;
        }

        SendCommand(new UpdateProcess()
        {
            Processes = MockUtil.MockProcesses(MockCount, MockPageSize)
        });
        Logger.Info("====TCP推送更新通知====");
    }

    #endregion
}