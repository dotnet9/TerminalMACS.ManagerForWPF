using SocketServer.Mock;

namespace SocketServer.ViewModels;

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

    private IAsyncCommand? _runCommand;

    /// <summary>
    /// 运行服务
    /// </summary>
    public IAsyncCommand RunCommand => _runCommand ??= new AsyncDelegateCommand(HandleRunCommandAsync,
        () => true);

    private IAsyncCommand? _sendCommand;

    /// <summary>
    /// 运行服务
    /// </summary>
    public IAsyncCommand SendCommand => _sendCommand ??= new AsyncDelegateCommand(
        HandleSendCommand,
        () => IsRunning).ObservesProperty(() => IsRunning);
    
    private Task HandleRunCommandAsync()
    {
        IsRunning = !IsRunning;
        if (IsRunning)
        {
            _tcpHelper.Start(TcpIp, TcpPort);
            _udpHelper.Start(UdpMulticastIp, UdpMulticastPort);
        }
        else
        {
            _tcpHelper.Stop();
            _udpHelper.Stop();
        }

        return Task.CompletedTask;
    }

    private Task HandleSendCommand()
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