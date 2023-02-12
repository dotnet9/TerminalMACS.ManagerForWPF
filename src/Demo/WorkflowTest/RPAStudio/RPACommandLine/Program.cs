using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPACommandLine.Interfaces;
using RPACommandLine.ServiceRegistry;
using CommandLine;
using RPACommandLine.Args;
using NLog;
using NLog.Targets;
using NLog.Config;
using System.IO;

namespace RPACommandLine
{
    //CMD要以管理员权限打开进行运行
    //命令行参数示例: -f "C:\Project\Main.xaml" -i "{'inArg':'value','integer':3}"  -l "log.txt"

    class Program
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private static IProjectService _projectService;

        private static IGlobalService _globalService;

        private static string[] Args;

        private static AppServiceRegistry _serviceRegistry = new RPACommandLineServiceRegistry();

        public static int Current_DispatcherUnhandledException { get; private set; }

        static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            Args = args;

            _serviceRegistry.RegisterServices();

            _projectService = _serviceRegistry.ResolveType<IProjectService>();
            _globalService = _serviceRegistry.ResolveType<IGlobalService>();

            return Parser.Default.ParseArguments<Options>(args)
              .MapResult(
                options => RunAndReturnExitCode(options),
                _ => 1);
        }


       
        public static void Stop()
        {
            _projectService.Stop();
        }

        static void changeLogPath(string logPath)
        {
            _logger.Debug($"日志文件切换为：{logPath}", _logger);

            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("File");
            fileTarget.FileName = logPath;
        }

        static int RunAndReturnExitCode(Options options)
        {
            _logger.Debug($"+++++++++++++++++命令行方式运行 RPACommandLine.exe+++++++++++++++++ 命令行参数为：{String.Join(" ", Args)}", _logger);

            _logger.Debug($"当前工作目录为：{System.IO.Directory.GetCurrentDirectory()}", _logger);

            if (!string.IsNullOrEmpty(options.Log))
            {
                changeLogPath(options.Log);
            }

            //设置命令行的当前路径为项目路径
            var projectDir = Path.GetDirectoryName(options.File);
            Directory.SetCurrentDirectory(projectDir);

            _globalService.Options = options;
            _projectService = _serviceRegistry.ResolveType<IProjectService>();
            _projectService.Init(options.File);
            _projectService.Start();
            _globalService.AutoResetEvent.WaitOne();

            return 0;
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    _logger.Error("非UI线程全局异常");
                    _logger.Error(exception);
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("不可恢复的非UI线程全局异常", _logger);
                _logger.Fatal(ex);
            }
        }



    }
}

