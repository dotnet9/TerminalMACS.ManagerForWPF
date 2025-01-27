using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TerminalMACS.Views;

/// <summary>
///     自定义启动窗体
/// </summary>
public partial class CusSplashScreen : Window
{
    private readonly ImageBrush backgroundBrush = new();
    private readonly DispatcherTimer timer = new();

    public CusSplashScreen()
    {
        InitializeComponent();
        myCanvas.Focus();

        timer.Tick += Engine;
        timer.Interval = TimeSpan.FromMilliseconds(20);
        backgroundBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Start.jpg"));
        background.Fill = backgroundBrush;
        background2.Fill = backgroundBrush;
        Start();
        Loaded += StartView_Loaded;
    }

    private void StartView_Loaded(object sender, RoutedEventArgs e)
    {
        var bw = new BackgroundWorker();

        bw.DoWork += (s, y) =>
        {
            // 长时间任务
            Thread.Sleep(2000);
        };

        bw.RunWorkerCompleted += (s, y) =>
        {
            tbMsg.Text = "开始体验";
            timer.Stop();

            var mView = new MainWindow();
            mView.Show();
            Application.Current.MainWindow = mView;

            var closeAnimation = new DoubleAnimation
            {
                From = Width,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseIn }
            };
            closeAnimation.Completed += (s1, e1) => { Close(); };
            //this.BeginAnimation(Window.OpacityProperty, closeAnimation);
            BeginAnimation(WidthProperty, closeAnimation);
        };
        tbMsg.Text = "即将进入";
        bw.RunWorkerAsync();
    }

    private void Engine(object sender, EventArgs e)
    {
        var backgroundLeft = Canvas.GetLeft(background) - 3;
        var background2Left = Canvas.GetLeft(background2) - 3;
        Canvas.SetLeft(background, backgroundLeft);
        Canvas.SetLeft(background2, background2Left);
        Canvas.SetLeft(tb1, Canvas.GetLeft(tb1) - 3);
        Canvas.SetLeft(tb2, Canvas.GetLeft(tb2) - 3);
        if (backgroundLeft <= -1262)
        {
            timer.Stop();
            Start();
        }
    }

    private void Start()
    {
        Canvas.SetLeft(background, 0);
        Canvas.SetLeft(background2, 1262);

        timer.Start();
    }
}