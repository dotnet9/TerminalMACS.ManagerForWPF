namespace SkiaSharpDemo;

public partial class MainWindow : Window
{
    public SKTypeface Font;
    public FiveRings FiveRings = new FiveRings();

    public MainWindow()
    {
        InitializeComponent();
        // 获取宋体在字体集合中的下标
        var index = SKFontManager.Default.FontFamilies.ToList().IndexOf("微软雅黑");
        Font = index < 0
            ? SKTypeface.Default
            :
            // 创建宋体字形
            SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);

        skContainer.PaintSurface += SkContainer_PaintSurface;
        skContainer.MouseDown += SkContainer_MouseDown;
        skContainer.MouseUp += SkContainer_MouseUp;
        skContainer.MouseMove += SkContainer_MouseMove;
        _ = Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    Dispatcher.Invoke(() => { skContainer.InvalidateVisual(); });
                    _ = SpinWait.SpinUntil(() => false, 1000 / 60); //每秒60帧
                }
                catch
                {
                    break;
                }
            }
        });
    }

    private void SkContainer_PaintSurface(object? sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        FiveRings.Render(canvas, Font, e.Info.Width, e.Info.Height);
    }

    private void SkContainer_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var cur = e.GetPosition(sender as IInputElement);
        FiveRings.MouseDown(new SKPoint((float)cur.X, (float)cur.Y));
    }

    private void SkContainer_MouseUp(object sender, MouseEventArgs e)
    {
        var cur = e.GetPosition(sender as IInputElement);
        FiveRings.MouseUp(new SKPoint((float)cur.X, (float)cur.Y));
    }

    private void SkContainer_MouseMove(object sender, MouseEventArgs e)
    {
        var cur = e.GetPosition(sender as IInputElement);
        FiveRings.MouseMove(new SKPoint((float)cur.X, (float)cur.Y));
    }
}