using SocketServer.Mock;

namespace SocketServer.ViewModels;

public class MainViewModel : BindableBase
{
    public TcpHelper TcpHelper { get; set; }
    public UdpHelper UdpHelper { get; set; }

    private IAsyncCommand? _runTcpCommand;

    /// <summary>
    /// 连接Tcp服务
    /// </summary>
    public IAsyncCommand RunTcpCommand =>
        _runTcpCommand ??= new AsyncDelegateCommand(HandleRunTcpCommandCommandAsync);

    private IAsyncCommand? _runUdpMulticastCommand;

    /// <summary>
    /// 订阅Udp组播
    /// </summary>
    public IAsyncCommand RunUdpMulticastCommand =>
        _runUdpMulticastCommand ??= new AsyncDelegateCommand(HandleRunUdpMulticastCommandAsync);

    private IAsyncCommand? _refreshCommand;

    /// <summary>
    /// 刷新数据
    /// </summary>
    public IAsyncCommand RefreshCommand => _refreshCommand ??= new AsyncDelegateCommand(
        HandleRefreshCommandAsync,
        () => TcpHelper.IsRunning).ObservesProperty(() => TcpHelper.IsRunning);

    private IAsyncCommand? _updateCommand;

    /// <summary>
    /// 更新数据
    /// </summary>
    public IAsyncCommand UpdateCommand => _updateCommand ??= new AsyncDelegateCommand(
        HandleUpdateCommandAsync,
        () => TcpHelper.IsRunning).ObservesProperty(() => TcpHelper.IsRunning);

    public MainViewModel()
    {
        TcpHelper = new TcpHelper();
        UdpHelper = new UdpHelper(TcpHelper);
    }

    private Task HandleRunTcpCommandCommandAsync()
    {
        if (!TcpHelper.IsStarted)
        {
            TcpHelper.Start();
        }
        else
        {
            TcpHelper.Stop();
        }

        return Task.CompletedTask;
    }

    private Task HandleRunUdpMulticastCommandAsync()
    {
        if (!UdpHelper.IsStarted)
        {
            UdpHelper.Start();
        }
        else
        {
            UdpHelper.Stop();
        }

        return Task.CompletedTask;
    }

    private Task HandleRefreshCommandAsync()
    {
        if (!TcpHelper.IsRunning)
        {
            Logger.Error("未运行Tcp服务，无法发送命令");
            return Task.CompletedTask;
        }

        TcpHelper.UpdateAllData(true);
        return Task.CompletedTask;
    }

    private Task HandleUpdateCommandAsync()
    {
        if (!TcpHelper.IsRunning)
        {
            Logger.Error("未运行Tcp服务，无法发送命令");
            return Task.CompletedTask;
        }

        TcpHelper.UpdateAllData(false);
        return Task.CompletedTask;
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