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
        return Task.CompletedTask;
    }

    private Task HandleSendCommand()
    {
        if (!TcpHelper.IsRunning)
        {
            Logger.Error("Tcp服务未运行，无法发送命令");
            return Task.CompletedTask;
        }

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
                    TcpHelper.SendCommand(command);
                }

                sw.Stop();
                Logger.Info($"已将{pageCount}个更新命令压入队列，用时{sw.ElapsedMilliseconds}ms");
            });
        });
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