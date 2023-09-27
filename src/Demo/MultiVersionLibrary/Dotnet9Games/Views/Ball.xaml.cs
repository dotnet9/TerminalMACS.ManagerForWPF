using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dotnet9Games.Views;

/// <summary>
/// 气球
/// </summary>
public partial class Ball : UserControl
{
    /// <summary>
    /// 气球最小尺寸
    /// </summary>
    private const int MinNumber = 20;

    /// <summary>
    /// 气球最大尺寸
    /// </summary>
    private const int MaxNumber = 120;

    /// <summary>
    /// 最大分
    /// </summary>
    private const int MaxScore = 12;

    /// <summary>
    /// 当前气球表示的分数
    /// </summary>
    public int Score { get; private set; }

    public Ball()
    {
        InitializeComponent();
        CreateBall();
    }

    /// <summary>
    /// 创建气球
    /// </summary>
    private void CreateBall()
    {
        var random = new Random(DateTime.Now.Millisecond);

        var randomSize = random.Next(MinNumber, MaxNumber);
        this.Width = this.Height = randomSize;
        Score = MaxScore - (randomSize / 10);
        TextBlockBallNumber.Text = $"{Score}";
        EllipseBall.ToolTip = $"{Score}号球";

        // 创建渐变色集合
        GradientStopCollection gradientStops = new()
        {
            new GradientStop(
                Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)),
                0),
            new GradientStop(
                Color.FromArgb(255, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)),
                0.66),
            new GradientStop(
                Color.FromArgb(0, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)), 1)
        };

        // 设置气球的填充为渐变色
        var fillBrush = new RadialGradientBrush(gradientStops)
        {
            RadiusX = 0.75,
            RadiusY = 0.75,
            GradientOrigin = new Point(0.2, 0.8),
            Center = new Point(0.5, 0.5)
        };
        EllipseBall.Fill = fillBrush;
    }
}