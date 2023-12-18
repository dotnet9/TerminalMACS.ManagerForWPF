using SocketCore.SysProcess.Models;
using SocketServer.Mock;

namespace SocketServer.SocketHeper;

public class UdpHelper(TcpHelper tcpHelper) : BindableBase, ISocketBase
{
    private UdpClient? _client;
    private IPEndPoint? _udpIpEndPoint;

    private BlockingCollection<INetObject> NeedSendCommands { get; } = new();

    #region 公开接口

    private string _ip = "224.0.0.0";

    /// <summary>
    /// UDP组播IP
    /// </summary>
    public string Ip
    {
        get => _ip;
        set => SetProperty(ref _ip, value);
    }

    private int _port = 9540;

    /// <summary>
    /// UDP组播端口
    /// </summary>
    public int Port
    {
        get => _port;
        set => SetProperty(ref _port, value);
    }

    private bool _isStarted;

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
    /// 是否正在运行udp组播订阅
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

    private int _packetMaxSize = 65507;

    /// <summary>
    /// Udp单包大小上限
    /// </summary>
    public int PacketMaxSize
    {
        get => _packetMaxSize;
        set => SetProperty(ref _packetMaxSize, value);
    }

    public void Start()
    {
        if (IsStarted)
        {
            Logger.Warning("Udp组播已经开启");
            return;
        }

        IsStarted = true;

        Task.Run(() =>
        {
            while (IsStarted)
            {
                try
                {
                    var ipAddress = IPAddress.Parse(Ip);
                    _udpIpEndPoint = new IPEndPoint(ipAddress, Port);
                    _client = new UdpClient();
                    _client.JoinMulticastGroup(ipAddress);
                    IsRunning = true;

                    SendCommands();
                    MockSendData();

                    Logger.Info($"Udp启动成功");
                    break;
                }
                catch (Exception ex)
                {
                    IsRunning = false;
                    Logger.Warning($"运行Udp异常，1秒后将重新运行：{ex.Message}");
                    Thread.Sleep(1);
                }
            }
        });
    }

    public void Stop()
    {
        if (!IsStarted)
        {
            Logger.Warning("Udp组播已经关闭");
            return;
        }

        IsStarted = false;

        try
        {
            _client?.Close();
            _client = null;
            Logger.Info($"停止Udp");
        }
        catch (Exception ex)
        {
            Logger.Warning($"停止Udp异常：{ex.Message}");
        }

        IsRunning = false;
    }

    public void SendCommand(INetObject command)
    {
        if (!IsRunning)
        {
            Logger.Error("Udp组播未运行，无法发送命令");
            return;
        }

        NeedSendCommands.Add(command);
    }

    public void SendCommandBuffer(byte[] buffer)
    {
        throw new NotImplementedException();
    }

    public bool TryGetResponse(out INetObject? response)
    {
        response = null;
        return false;
    }

    #endregion

    private void SendCommands()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                if (!NeedSendCommands.TryTake(out var command, TimeSpan.FromMilliseconds(1)))
                {
                    continue;
                }

                try
                {
                    var buffer = command.Serialize(tcpHelper.SystemId);
                    _client?.Send(buffer, buffer.Length, _udpIpEndPoint);
                }
                catch (Exception ex)
                {
                    NeedSendCommands.Add(command);
                    Logger.Error($"发送命令{command.GetType().Name}失败，将排队重新发送: {ex.Message}");
                }
            }
        });
    }

    private void MockSendData()
    {
        Task.Run(() =>
        {
            while (IsRunning)
            {
                MockUtil.MockUpdateActiveProcessPageCount(tcpHelper.MockCount, tcpHelper.MockPageSize, out var pageSize,
                    out var pageCount);
                for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
                {
                    if (!IsRunning)
                    {
                        break;
                    }

                    var response = new UpdateActiveProcess
                    {
                        Processes = Enumerable
                            .Range(pageIndex * pageSize,
                                MockUtil.GetDataCount(tcpHelper.MockCount, pageSize, pageIndex))
                            .Select(index => new ActiveProcess()
                            {
                                PID = index + 1,
                                CPUUsage = Random.Shared.NextDouble(),
                                MemoryUsage = Random.Shared.NextDouble(),
                                DiskUsage = Random.Shared.NextDouble(),
                                NetworkUsage = Random.Shared.NextDouble(),
                                GPU = Random.Shared.NextDouble(),
                                PowerUsage =
                                    (byte)(Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length)),
                                PowerUsageTrend =
                                    (byte)(Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length))
                            }).ToList()
                    };
                    SendCommand(response);
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }
        });
    }
}