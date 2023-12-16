using SocketClient.Mock;
using SocketClient.Models;
using SocketClient.WPF;

namespace SocketClient.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly TcpHelper _tcpHelper = new();
    private readonly UdpHelper _udpHelper = new();
    private string _tcpIp = "127.0.0.1";

    /// <summary>
    /// TCP服务IP
    /// </summary>
    public string TcpIp
    {
        get => _tcpIp;
        set => SetProperty(ref _tcpIp, value);
    }

    /// <summary>
    /// TCP服务端口
    /// </summary>
    private int _tcpPort = 5000;

    public int TcpPort
    {
        get => _tcpPort;
        set => SetProperty(ref _tcpPort, value);
    }

    private string _udpMulticastIp = "224.0.0.0";

    /// <summary>
    /// UDP组播放IP
    /// </summary>
    public string UdpMulticastIp
    {
        get => _udpMulticastIp;
        set => SetProperty(ref _udpMulticastIp, value);
    }

    private int _udpMulticastPort = 9540;

    /// <summary>
    /// UDP组播端口
    /// </summary>
    public int UdpMulticastPort
    {
        get => _udpMulticastPort;
        set => SetProperty(ref _udpMulticastPort, value);
    }

    private bool _isRunning;

    /// <summary>
    /// 是否正在运行服务
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

    private readonly List<ProcessItem> _receivedProcesses = new();

    private Dictionary<int, ProcessItem>? _processIdAndItems;

    public RangObservableCollection<ProcessItem> DisplayProcesses { get; } = new();

    private IAsyncCommand? _runCommand;

    /// <summary>
    /// 运行服务
    /// </summary>
    public IAsyncCommand RunCommand => _runCommand ??= new AsyncDelegateCommand(HandleRunCommandAsync,
        () => true);

    private IAsyncCommand? _refreshCommand;

    /// <summary>
    /// 运行服务
    /// </summary>
    public IAsyncCommand RefreshCommand => _refreshCommand ??= new AsyncDelegateCommand(
        HandleRefreshCommand,
        () => IsRunning).ObservesProperty(() => IsRunning);

    private Task HandleRunCommandAsync()
    {
        IsRunning = !IsRunning;
        if (IsRunning)
        {
            _tcpHelper.Start(TcpIp, TcpPort);
            _udpHelper.Start(UdpMulticastIp, UdpMulticastPort);

            ReceiveTcpData();
            ReceiveUdpData();
            SendHeartbeat();
            UpdateCount();
        }
        else
        {
            _tcpHelper.Stop();
            _udpHelper.Stop();
        }

        return Task.CompletedTask;
    }

    private Task HandleRefreshCommand()
    {
        SendUpdateProcess();
        return Task.CompletedTask;
    }

    private void SendUpdateProcess()
    {
        Task.Run(() =>
        {
            Try("推送进程信息", () =>
            {
                var sw = Stopwatch.StartNew();
                var pageCount = MockUtil.GetPageCount(MockUtil.MockCount, MockUtil.MockPageSize);
                for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
                {
                    var command = new ResponseProcess()
                    {
                        Processes = MockUtil.MockProcesses(pageIndex)
                    };
                    _tcpHelper.SendCommand(command);
                }

                sw.Stop();
                Logger.Info($"已将{pageCount}个更新命令压入队列，用时{sw.ElapsedMilliseconds}ms");
            });
        });
    }


    #region 读取Tcp数据

    private void ReceiveTcpData()
    {
        // 开启线程接收数据
        Task.Run(() =>
        {
            while (!IsRunning)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }

            HandleRefreshCommand();

            while (true)
            {
                Try("读取TCP数据", ReadTcpData, ex => Logger.Error($"循环处理数据异常：{ex.Message}"));

                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }

    private void ReadTcpData()
    {
        if (!_tcpHelper.TryGetResponse(out var command))
        {
            return;
        }

        switch (command)
        {
            case ResponseBaseInfo responseBase:
                ReadResponse(responseBase);
                break;
            case ResponseProcess responseProcess:
                ReadResponse(responseProcess);
                break;
            case UpdateProcess responseProcess:
                ReadResponse(responseProcess);
                break;
            case Heartbeat responseHeartbeat:
                ReadResponse(responseHeartbeat);
                break;
            default:
                throw new Exception($"视图未处理新的数据包{command!.GetType().Name}");
        }
    }

    private void ReadResponse(ResponseBaseInfo response)
    {
    }

    private void ReadResponse(ResponseProcess response)
    {
    }

    private void ReadResponse(UpdateProcess response)
    {
    }

    private void ReadResponse(Heartbeat response)
    {
    }

    #endregion

    #region 接收Udp数据

    private void ReceiveUdpData()
    {
        Task.Run(() =>
        {
            while (true)
            {
                if (_udpHelper.TryGetResponse(out var response) && response is UpdateActiveProcess updateActiveProcess)
                {
                    try
                    {
                        DillUpdateActivePoints(updateActiveProcess);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"更新点实时数据异常：{ex.Message}");
                    }
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }

    private void DillUpdateActivePoints(UpdateActiveProcess updateActiveProcess)
    {
    }

    #endregion

    private void SendHeartbeat()
    {
        Task.Run(() =>
        {
            while (true)
            {
                if (TcpHelper.IsRunning)
                {
                    _tcpHelper.SendCommand(new Heartbeat());
                    Logger.Info("向服务端发送心跳");
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        });
    }

    private void UpdateCount()
    {
        Task.Run(() =>
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(30));
            }
        });
    }

    private void Try(string actionName, Action action, Action<Exception>? exceptionAction = null
    )
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            if (exceptionAction == null)
            {
                Logger.Error($"执行{actionName}异常：{ex.Message}");
            }
            else
            {
                exceptionAction.Invoke(ex);
            }
        }
    }
}