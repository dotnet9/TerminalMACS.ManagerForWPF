using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfThemeDemo;

public partial class MainWindow : Window
{
    private const int MousePullInfoIntervalInMs = 10;
    private readonly DispatcherTimer _timer = new DispatcherTimer();

    public MainWindow()
    {
        InitializeComponent();
        _timer.Interval = TimeSpan.FromMilliseconds(MousePullInfoIntervalInMs);
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
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var isMouseDown = GetCursorPos(out var point);
        var color = GetPixelColor((int)point.X, (int)point.Y);
        TxtColorPoint.Text = $"x:{point.X}   y:{point.Y}";
        BorderColor.Background = new SolidColorBrush(color);
    }


    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

    public static Color GetPixelColor(int x, int y)
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        uint pixel = GetPixel(hdc, x, y);
        ReleaseDC(IntPtr.Zero, hdc);
        Color color = Color.FromRgb(
            (byte)(pixel & 0x000000FF),
            (byte)((pixel & 0x0000FF00) >> 8),
            (byte)((pixel & 0x00FF0000) >> 16));
        return color;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern bool GetCursorPos(out PointStruct pt);

    [StructLayout(LayoutKind.Sequential)]
    internal struct PointStruct
    {
        public int X;
        public int Y;
    }
}