using QuickApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuickApp.Views
{
    /// <summary>
    /// MainWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        private Rect desktopWorkingArea;

        System.Windows.Point anchorPoint;
        bool inDrag;

        public MainWindow2()
        {
            InitializeComponent();
            //if (!Microsoft.Windows.Shell.SystemParameters2.Current.IsGlassEnabled)
            //{
            //   
            //}

            desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Height = desktopWorkingArea.Height / 2;
            this.Left = desktopWorkingArea.Width - this.Width;
            this.Top = desktopWorkingArea.Height / 2 - (this.Height / 2);
            this.Loaded += Window_Loaded;
            this.Deactivated += MainView_Deactivated;
        }

        #region Deactivated
        private void MainView_Deactivated(object sender, EventArgs e)
        {
            MainWindow2 window = (MainWindow2)sender;
            window.Topmost = true;
        }
        #endregion

        #region Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.IsWin10)
            {
                //WindowBlur.SetIsEnabled(this, true);
                //DataContext = new WindowBlureffect(this, AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND) { BlurOpacity = 100 };
            }
            //if (Environment.OSVersion.Version.Major >= 6)
            //    Win32Api.SetProcessDPIAware();

            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)Win32Api.GetWindowLong(wndHelper.Handle, (int)Win32Api.GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)Win32Api.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            Win32Api.SetWindowLong(wndHelper.Handle, (int)Win32Api.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
            //Win32Api.HwndSourceAdd(this);
        }
        #endregion

        #region 移动窗体
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            anchorPoint = e.GetPosition(this);
            inDrag = true;
            CaptureMouse();
            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                if (inDrag)
                {
                    System.Windows.Point currentPoint = e.GetPosition(this);
                    var y = this.Top + currentPoint.Y - anchorPoint.Y;
                    Win32Api.RECT rect;
                    Win32Api.GetWindowRect(new WindowInteropHelper(this).Handle, out rect);
                    var w = rect.right - rect.left;
                    var h = rect.bottom - rect.top;
                    int x = Convert.ToInt32(PrimaryScreen.DESKTOP.Width - w);

                    Win32Api.MoveWindow(new WindowInteropHelper(this).Handle, x, (int)y, w, h, 1);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MainView.OnMouseMove{ex.Message}");
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (inDrag)
            {
                ReleaseMouseCapture();
                inDrag = false;
                e.Handled = true;
            }
        }
        #endregion

        #region 窗体动画
        private void ToggleButtonMini_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                EasingFunctionBase easeFunction = new CubicEase()
                {
                    EasingMode = EasingMode.EaseOut,
                };

                var heightAnimation = new DoubleAnimation
                {
                    Name = "heightMini",
                    To = 60,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = easeFunction
                };
                var widthAnimation = new DoubleAnimation
                {
                    Name = "widthMini",
                    To = 30,
                    Duration = new Duration(TimeSpan.FromSeconds(0.51)),
                    EasingFunction = easeFunction
                };
                widthAnimation.Completed += (s, e1) =>
                {
                    this.Left = desktopWorkingArea.Width - this.Width;
                };
                //heightAnimation.Completed += Animation_Completed;
                //widthAnimation.Completed += Animation_Completed;
                this.BeginAnimation(Window.HeightProperty, heightAnimation);
                this.BeginAnimation(Window.WidthProperty, widthAnimation);
            }
            catch (Exception ex)
            {
                Log.Error($"MainView.ToggleButtonMini_Checked{ex.Message}");
            }
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            Timeline name = ((AnimationClock)sender).Timeline;
            switch (name.Name)
            {
                case "widthMini":
                    this.Left = desktopWorkingArea.Width - this.Width;
                    break;

            }
            Console.WriteLine(this.Width);
        }

        private void UnToggleButtonMini_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                EasingFunctionBase easeFunction = new CubicEase()
                {
                    EasingMode = EasingMode.EaseIn,
                };
                var widthAnimation = new DoubleAnimation
                {
                    To = 120,
                    Duration = new Duration(TimeSpan.FromSeconds(0.01)),
                    EasingFunction = easeFunction
                };
                widthAnimation.Completed += (s, e1) =>
                {
                    this.Left = desktopWorkingArea.Width - this.Width;
                };

                var heightAnimation = new DoubleAnimation
                {
                    To = desktopWorkingArea.Height / 2,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = easeFunction
                };
                this.BeginAnimation(Window.WidthProperty, widthAnimation);
                this.BeginAnimation(Window.HeightProperty, heightAnimation);
            }
            catch (Exception ex)
            {
                Log.Error($"MainView.UnToggleButtonMini_Checked{ex.Message}");
            }

        }
        #endregion


    }

}