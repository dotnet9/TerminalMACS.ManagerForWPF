using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using WorkflowCore.Interface;

namespace WorkflowTest;

public partial class MainWindow : Window
{
    IServiceProvider? _serviceProvider = null;
    bool _serviceStarted = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartWorkflow()
    {
        if (_serviceProvider == null)
        {
            _serviceProvider = ConfigureServices();
            var host1 = _serviceProvider.GetService<IWorkflowHost>();

            host1?.RegisterWorkflow<HelloWorkflow>();
            host1?.RegisterWorkflow<HelloWorkflow2>();
        }


        var host = _serviceProvider.GetService<IWorkflowHost>();
        var wd = host.Registry.GetDefinition("HelloWorkflow");

        // 如果host启动了，不能再次启动，但没有判断方法
        if (!_serviceStarted)
        {
            host.Start();
            _serviceStarted = true;
        }


        // 启动workflow工作流
        host.StartWorkflow("HelloWorkflow", 1, data: null); //
        //host.StartWorkflow("HelloWorkflow");//, 2, data: null, 默认会启用版本高的
    }

    private void StopWorkflow()
    {
        var host = _serviceProvider.GetService<IWorkflowHost>();

        host?.Stop();
        _serviceStarted = false;
    }

    /// <summary>
    /// 配置workflow
    /// </summary>
    /// <returns></returns>
    private IServiceProvider ConfigureServices()
    {
        //setup dependency injection
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddWorkflow();
        //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));

        // 这些个构造函数带参数的，需要添加到transient中
        services.AddTransient<HelloWorld>();
        services.AddTransient<GoodbyeWorld>();
        services.AddTransient<SleepStep>();

        var serviceProvider = services.BuildServiceProvider();

        //config logging
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        loggerFactory?.AddProvider(new DebugLoggerProvider());

        return serviceProvider;
    }

    private void startButton_Click(object sender, RoutedEventArgs e)
    {
        StartWorkflow();
    }

    private void stopButton_Click(object sender, RoutedEventArgs e)
    {
        StopWorkflow();
    }
}