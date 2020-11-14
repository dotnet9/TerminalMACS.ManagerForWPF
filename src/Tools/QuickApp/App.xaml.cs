using QuickApp.Helpers;
using QuickApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace QuickApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public EventWaitHandle ProgramStarted { get; set; }
        private string appName;
        protected override void OnStartup(StartupEventArgs e)
        {
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            appName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, appName, out var createNew);
            if (!createNew)
            {
                // 唤起已经启动的进程
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        Win32Api.SetForegroundWindow(process.MainWindowHandle);
                        App.Current.Shutdown();
                        Environment.Exit(-1);
                        break;
                    }
                }

            }
            else
            {
                Common common = new Common();
                var main = new MainWindow2();
                main.Show();
            }

            base.OnStartup(e);

            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }


        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出      
                Log.Error("UI线程异常:" + e.Exception.Message);
            }
            catch (Exception ex)
            {
                //此时程序出现严重异常，将强制结束退出
                Log.Error("UI线程发生致命错误！");
            }

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("非UI线程发生致命错误");
            }
            sbEx.Append("非UI线程异常：");
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            Log.Error(sbEx.ToString());
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            Log.Error("Task线程异常：" + e.Exception.Message);
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }
    }
}
