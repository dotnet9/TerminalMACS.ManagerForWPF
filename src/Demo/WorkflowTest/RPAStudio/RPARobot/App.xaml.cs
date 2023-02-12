using NLog;
using RPA.Shared.Utils;
using RPARobot.ServiceRegistry;
using RPARobot.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RPARobot
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// mutex，控制程序单实例运行
        /// </summary>
        private Mutex instanceMutex = null;

        private static AppServiceRegistry _serviceRegistry = new RPARobotServiceRegistry();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Init();
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            UnInit();
        }


        private void Init()
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _serviceRegistry.RegisterServices();

            //单程序实例控制
            bool createdNew = false;
            instanceMutex = new Mutex(true, "{59C0FF9E-1999-40B8-9499-A48D1BFD6BC8}", out createdNew);
            if (createdNew)
            {
#if DEBUG
                Common.OpenConsole();
#endif

                _logger.Debug($"RPARobot v{Common.GetProgramVersion()}启动……");

            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void UnInit()
        {

#if DEBUG
            Common.CloseConsole();
#endif
        }


        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                _logger.Error("UI线程全局异常");
                _logger.Error(e.Exception);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                _logger.Fatal("不可恢复的UI线程全局异常");
                _logger.Fatal(ex);
            }

            Common.InvokeOnUI(() =>
            {
                CommonMessageBox.ShowError(e.Exception.ToString());
            });
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
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
                _logger.Fatal("不可恢复的非UI线程全局异常");
                _logger.Fatal(ex);
            }

            Common.InvokeOnUI(() =>
            {
                CommonMessageBox.ShowError((e.ExceptionObject as Exception).ToString());
            });
        }


    }
}