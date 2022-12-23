using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Vanara.PInvoke;
using Color = System.Windows.Media.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;

namespace WpfThemeDemo;

public partial class MainWindow : Window
{
    internal const int WM_MOUSEMOVE = 0x200;
    private readonly DispatcherTimer _timer = new();
    private POINT _currentPoint;
    private User32.HookProc? _moveBoardHookProcedure;
    private User32.SafeHHOOK? _moveHookStatus;

    public MainWindow()
    {
        InitializeComponent();
        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Tick += Timer_Tick;
        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
    }

    private void ChangeTheme_Click(object sender, RoutedEventArgs e)
    {
        ResourceDictionary resource = new();
        if (Application.Current.Resources.MergedDictionaries[0].Source.ToString() ==
            "pack://application:,,,/WpfThemeDemo;component/Resources/Light.xaml")
        {
            resource.Source = new Uri("pack://application:,,,/WpfThemeDemo;component/Resources/Dark.xaml");
        }
        else
        {
            resource.Source = new Uri("pack://application:,,,/WpfThemeDemo;component/Resources/Light.xaml");
        }

        Application.Current.Resources.MergedDictionaries[0] = resource;
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _timer.Stop();
        if (this._moveHookStatus != null)
        {
            User32.UnhookWindowsHookEx(this._moveHookStatus);
            this._moveHookStatus = null;
        }
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _timer.Start();

        if (this._moveHookStatus == null)
        {
            this._moveBoardHookProcedure = this.MouseHookProc;
            this._moveHookStatus = User32.SetWindowsHookEx(User32.HookType.WH_MOUSE_LL, this._moveBoardHookProcedure);
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        //var isMouseDown = User32.GetCursorPos(out var point);
        // 上面的取点在操作系统分辨率为175%等比例缩放时会有问题，所以使用下面的方法
        var point = this._currentPoint;
        var color = GetColorFromPoint(new Point(point.X, point.Y));
        TxtColorPoint.Text = $"x:{point.X}   y:{point.Y}";
        BorderColor.Background = new SolidColorBrush(color);
    }

    [SecuritySafeCritical]
    internal static Color GetColorFromPoint(Point point)
    {
        var pixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        using (var gDest = Graphics.FromImage(pixel))
        {
            using (var gSrc = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hSrcDC = gSrc.GetHdc();
                var hDC = gDest.GetHdc();
                Gdi32.BitBlt(hDC, 0, 0, 1, 1, hSrcDC, point.X, point.Y, Gdi32.RasterOperationMode.SRCCOPY);
                gDest.ReleaseHdc();
                gSrc.ReleaseHdc();
            }
        }

        var color = pixel.GetPixel(0, 0);
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    private IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        var mouseHookStruct = Marshal.PtrToStructure<User32.MOUSEHOOKSTRUCT>(lParam);
        if (wParam.ToInt32() == WM_MOUSEMOVE)
        {
            _currentPoint = mouseHookStruct.pt;
        }

        return User32.CallNextHookEx(this._moveHookStatus, nCode, wParam, lParam);
    }
}