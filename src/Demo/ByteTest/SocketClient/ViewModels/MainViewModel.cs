using SocketClient.Models;
using SocketClient.WPF;
using System.Windows;
using SocketCore.Utils;

namespace SocketClient.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly List<ProcessItem> _receivedProcesses = new();

    private Dictionary<int, ProcessItem>? _processIdAndItems;

    public RangObservableCollection<ProcessItem> DisplayProcesses { get; } = new();

    public TcpHelper TcpHelper { get; set; }
    public UdpHelper UdpHelper { get; set; }

    private string? _baseInfo;

    /// <summary>
    /// 基本信息
    /// </summary>
    public string? BaseInfo
    {
        get => _baseInfo;
        set => SetProperty(ref _baseInfo, value);
    }

    private IAsyncCommand? _connectTcpCommand;

    /// <summary>
    /// 连接Tcp服务
    /// </summary>
    public IAsyncCommand ConnectTcpCommand =>
        _connectTcpCommand ??= new AsyncDelegateCommand(HandleConnectTcpCommandAsync);

    private IAsyncCommand? _subscribeUdpMulticastCommand;

    /// <summary>
    /// 订阅Udp组播
    /// </summary>
    public IAsyncCommand SubscribeUdpMulticastCommand =>
        _subscribeUdpMulticastCommand ??= new AsyncDelegateCommand(HandleSubscribeUdpMulticastCommand);

    private IAsyncCommand? _refreshCommand;

    /// <summary>
    /// 刷新数据
    /// </summary>
    public IAsyncCommand RefreshCommand => _refreshCommand ??= new AsyncDelegateCommand(
        HandleRefreshCommand,
        () => TcpHelper.IsRunning).ObservesProperty(() => TcpHelper.IsRunning);

    public MainViewModel()
    {
        TcpHelper = new TcpHelper();
        UdpHelper = new UdpHelper(TcpHelper);

        UpdateCount();
    }

    private Task HandleConnectTcpCommandAsync()
    {
        if (!TcpHelper.IsStarted)
        {
            TcpHelper.Start();

            ReceiveTcpData();
            SendHeartbeat();
        }
        else
        {
            TcpHelper.Stop();
        }

        return Task.CompletedTask;
    }

    private Task HandleSubscribeUdpMulticastCommand()
    {
        if (!UdpHelper.IsStarted)
        {
            UdpHelper.Start();

            ReceiveUdpData();
        }
        else
        {
            UdpHelper.Stop();
        }

        return Task.CompletedTask;
    }

    private Task HandleRefreshCommand()
    {
        TcpHelper.SendCommand(new RequestBaseInfo { TaskId = TcpHelper.GetNewTaskId() });
        return Task.CompletedTask;
    }


    #region 读取Tcp数据

    private void ReceiveTcpData()
    {
        // 开启线程接收数据
        Task.Run(() =>
        {
            while (!TcpHelper.IsRunning)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            HandleRefreshCommand();

            while (TcpHelper.IsRunning)
            {
                Try("读取TCP数据", ReadTcpData, ex => Logger.Error($"循环处理数据异常：{ex.Message}"));

                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }

    private void ReadTcpData()
    {
        if (!TcpHelper.TryGetResponse(out var command))
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
        BaseInfo =
            $"更新时间【{response.LastUpdateTime.ToDateTime():yyyy:MM:dd HH:mm:ss fff}】：数据中心【{response.DataCenterLocation}】-操作系统【{response.OperatingSystemType}】-内存【{ByteSizeConverter.FormatMB(response.MemorySize)}】-处理器个数【{response.ProcessorCount}】-硬盘【{ByteSizeConverter.FormatGB(response.TotalDiskSpace)}】-带宽【{response.NetworkBandwidth}Mbps】";
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
            while (UdpHelper.IsRunning)
            {
                if (UdpHelper.TryGetResponse(out var response) && response is UpdateActiveProcess updateActiveProcess)
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
            while (TcpHelper.IsRunning)
            {
                UdpHelper.SendCommand(new Heartbeat());
                Logger.Info("向服务端发送心跳");

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        });
    }

    private void UpdateCount()
    {
        Task.Run(() =>
        {
            while (UdpHelper.IsRunning)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(30));
            }
        });
    }

    private void Try(string actionName, Action action, Action<Exception>? exceptionAction = null)
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

    private void Invoke(Action action)
    {
        Application.Current.MainWindow?.Dispatcher.Invoke(action);
    }
}