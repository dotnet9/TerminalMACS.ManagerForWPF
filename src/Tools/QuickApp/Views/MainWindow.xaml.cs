using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Deactivated += MainWindow_Deactivated;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            MainWindow window = (MainWindow)sender;
            window.Topmost = true;
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Top = desktopWorkingArea.Top;
            tbSwitch_Click(null, null);
        }
        /// <summary>
        /// 开机启动和取消开机启动
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="f"></param>
        void RegRun(string appName, bool f)
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            bool b = false;
            foreach (string i in Run.GetValueNames())
            {
                if (i == appName)
                {
                    b = true;
                    break;
                }
            }
            try
            {
                if (f)
                {
                    //Run.SetValue(appName, Process.GetCurrentProcess().MainModule.FileName + " -autostart");
                    Run.SetValue(appName, "\"" + Process.GetCurrentProcess().MainModule.FileName + "\"");
                }
                else
                {
                    Run.DeleteValue(appName);
                }
            }
            catch
            { }
            HKCU.Close();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        Process p = new Process();
        /// <summary>
        /// 判断 当前运行电脑是64位系统还是32
        /// </summary>
        /// <returns></returns>
        public RegistryKey is64()
        {
            RegistryKey hkml;
            bool is64 = Environment.Is64BitOperatingSystem;
            if (is64 == true)
            {
                hkml = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64);
            }
            else
            {
                hkml = Registry.CurrentUser;
            }
            return hkml;
        }

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 根据“精确进程名”打开进程
        /// </summary>
        /// <param name="strProcName">精确进程名</param>
        public bool KillProc(string strProcName)
        {
            string pName = strProcName;//要启动的进程名称，可以在任务管理器里查看，一般是不带.exe后缀的;  
            var prc = Process.GetProcessesByName(pName);//在所有已启动的进程中查找需要的进程；  
            if (prc.Length > 0)//如果查找到  
            {
                SetForegroundWindow(prc[0].MainWindowHandle);    // 激活，显示在最前 
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 打开QQ与微信
        /// </summary>
        /// <returns></returns>
        public RegistryKey registryPublic()
        {
            RegistryKey bugReport;
            RegistryKey hkml = is64();
            RegistryKey software = hkml.OpenSubKey("Software", true);
            RegistryKey tencent = software.OpenSubKey("Tencent", true);
            return bugReport = tencent.OpenSubKey("bugReport", true);
        }
        public void TimerClose()
        {
            this.gridMeassage.Visibility = Visibility.Visible;
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    this.gridMeassage.Visibility = Visibility.Collapsed;
                }));
                timer.Stop();
            };
            timer.Start();
        }
        private void btnQQ_Click(object sender, RoutedEventArgs e)
        {
            //if (!KillProc("QQ"))
            //{

            //}
            try
            {
                RegistryKey bugReport = registryPublic();
                RegistryKey qq = bugReport.OpenSubKey("QQUrlMgr", true);
                string registPath = qq.GetValue("InstallDir").ToString();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = registPath + @"\QQ.exe";
                p.Start();
            }
            catch (Exception ex)
            {
                TimerClose();
            }
        }

        private void btnWeChat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!KillProc("WeChat"))
                //{

                //}
                RegistryKey bugReport = registryPublic();
                RegistryKey wechat = bugReport.OpenSubKey("WechatWindows", true);
                string registPath = wechat.GetValue("InstallDir").ToString();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = registPath + @"\WeChat.exe";
                p.Start();
            }
            catch (Exception ex)
            {
                TimerClose();
            }
        }
        /// <summary>
        /// 打开vs2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVisualStudio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryKey hkml = is64();
                RegistryKey software = hkml.OpenSubKey("Software", true);
                RegistryKey microsoft = software.OpenSubKey("Microsoft", true);
                RegistryKey visualStudio = microsoft.OpenSubKey("VisualStudio", true);
                if (visualStudio.SubKeyCount > 0)
                {
                    string[] subkeyNames;
                    subkeyNames = visualStudio.GetSubKeyNames();
                    foreach (string ikeyName in subkeyNames)
                    {
                        if (ikeyName == "12.0_Config")
                        {
                            RegistryKey config = visualStudio.OpenSubKey("12.0_Config", true);
                            string registPath = config.GetValue("InstallDir").ToString();
                            p.StartInfo.UseShellExecute = true;
                            p.StartInfo.FileName = registPath + @"\devenv.exe";
                            p.Start();
                            break;
                        }
                        else if (ikeyName == "14.0_Config")
                        {
                            RegistryKey config = visualStudio.OpenSubKey("14.0_Config", true);
                            string registPath = config.GetValue("InstallDir").ToString();
                            p.StartInfo.UseShellExecute = true;
                            p.StartInfo.FileName = registPath + @"\devenv.exe";
                            p.Start();
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TimerClose();
            }
        }
        /// <summary>
        /// google
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoogle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = "chrome.exe";
                p.Start();
            }
            catch (Exception ex)
            {
                TimerClose();
            }
        }
        /// <summary>
        /// 163Music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMusic163_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryKey hkml;
                bool is64 = Environment.Is64BitOperatingSystem;
                if (is64 == true)
                {
                    hkml = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
                    RegistryKey wow6432Node = software.OpenSubKey("Wow6432Node", true);
                    RegistryKey netease = wow6432Node.OpenSubKey("Netease", true);
                    RegistryKey cloudmusic = netease.OpenSubKey("cloudmusic", true);
                    string registPath = cloudmusic.GetValue("install_dir").ToString();
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.FileName = registPath + @"\cloudmusic.exe";
                    p.Start();
                }
                else
                {
                    hkml = Registry.LocalMachine;
                }

            }
            catch (Exception ex)
            {
                TimerClose();

            }
        }
        /// <summary>
        /// notepad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNotepad_Click(object sender, RoutedEventArgs e)
        {
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = "notepad.exe";
            p.Start();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnOpenOrClose_Click(object sender, RoutedEventArgs e)
        {
            if (btnOpenOrClose.IsChecked == true)
            {
                DoubleAnimation StopAnimation = new DoubleAnimation();
                StopAnimation.From = 0;
                StopAnimation.To = -80;
                StopAnimation.Duration = new Duration(TimeSpan.Parse("0:0:0.5"));
                _this.BeginAnimation(MainWindow.TopProperty, StopAnimation);
                btnOpenOrClose.ToolTip = "展开";
            }
            else
            {
                DoubleAnimation OpenAnimation = new DoubleAnimation();
                OpenAnimation.From = -80;
                OpenAnimation.To = 0.1;
                OpenAnimation.Duration = new Duration(TimeSpan.Parse("0:0:0.5"));
                _this.BeginAnimation(MainWindow.TopProperty, OpenAnimation);
                btnOpenOrClose.ToolTip = "收起";
            }
        }

        private void tbSwitch_Click(object sender, RoutedEventArgs e)
        {
            RegRun("SoftWareHelper", tbSwitch.IsChecked.Value);
        }

        private void menuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}