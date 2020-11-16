using QuickApp.Helpers;
using QuickApp.Views;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;

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
                var main = new Shell();
                main.Show();
            }

            base.OnStartup(e);

        }
    }
}
