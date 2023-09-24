using Dotnet9Games.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Dotnet9Games.Views;

/// <summary>
///     气球小游戏，只用于测试Lib.Harmony拦截
/// </summary>
public partial class BallGame : UserControl
{
    public static readonly DependencyProperty BallCountProperty =
        DependencyProperty.Register(nameof(BallCount), typeof(int), typeof(BallGame), new PropertyMetadata(0));

    private readonly List<Balloon> _balloons = new();

    private readonly Random _random = new(DateTime.Now.Millisecond);


    public BallGame()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     生成的气球个数
    /// </summary>
    public int BallCount
    {
        get => (int)GetValue(BallCountProperty);
        set => SetValue(BallCountProperty, value);
    }

    /// <summary>
    ///     开始游戏，比如生成飘散的彩色气球或播放爆炸动画
    /// </summary>
    public void StartGame()
    {
        if (BallCount > 9)
        {
            // 播放爆炸动画效果
            PlayBrokenHeartAnimation();
        }
        else
        {
            // 生成彩色气球
            GenerateBalloons();
        }
    }

    /// <summary>
    ///     播放心碎动画效果
    /// </summary>
    private void PlayBrokenHeartAnimation()
    {
        ClearBalloonAndBomb();

        // 设置炸弹的初始状态
        var ellipseBomb = new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = new SolidColorBrush(Colors.White)
        };

        CanvasPlayground.Children.Add(ellipseBomb);

        var maxSize = Math.Min(ActualWidth, ActualHeight);
        Canvas.SetLeft(ellipseBomb, (ActualWidth - maxSize) / 2);
        Canvas.SetTop(ellipseBomb, (ActualHeight - maxSize) / 2);

        // 创建动画
        DoubleAnimation sizeAnimation = new()
        {
            From = 10,
            To = maxSize,
            Duration = TimeSpan.FromSeconds(1.5)
        };

        ColorAnimation colorAnimation = new()
        {
            From = Colors.White,
            To = Colors.Red,
            Duration = TimeSpan.FromSeconds(1.5)
        };

        // 设置动画的缓动函数
        ElasticEase elasticEase = new()
        {
            Oscillations = 2,
            Springiness = 10
        };
        sizeAnimation.EasingFunction = elasticEase;

        // 启动动画
        ellipseBomb.BeginAnimation(Ellipse.WidthProperty, sizeAnimation);
        ellipseBomb.BeginAnimation(Ellipse.HeightProperty, sizeAnimation);
        ellipseBomb.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
    }


    /// <summary>
    ///     生成彩色气球
    /// </summary>
    private void GenerateBalloons()
    {
        ClearBalloonAndBomb();

        for (int i = 0; i < BallCount; i++)
        {
            // 生成彩色气球
            Ellipse balloonShape = new();
            balloonShape.Width = balloonShape.Height = _random.Next(50, 120);

            // 创建渐变色集合
            GradientStopCollection gradientStops = new()
            {
                new GradientStop(
                    Color.FromArgb(255, (byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256)),
                    0),
                new GradientStop(
                    Color.FromArgb(255, (byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256)),
                    0.66),
                new GradientStop(
                    Color.FromArgb(0, (byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256)), 1)
            };

            // 设置气球的填充为渐变色
            var fillBrush = new RadialGradientBrush(gradientStops)
            {
                RadiusX = 0.75,
                RadiusY = 0.75,
                GradientOrigin = new Point(0.2, 0.8),
                Center = new Point(0.5, 0.5)
            };
            balloonShape.Fill = fillBrush;

            // 设置气球的位置和速度
            double x = _random.Next((int)ActualWidth - 50);
            double y = _random.Next((int)ActualHeight - 50);
            double speedX = _random.NextDouble() * 2 - 1;
            double speedY = _random.NextDouble() * 2 - 1;

            // 将气球添加到用户控件上的某个容器中
            CanvasPlayground.Children.Add(balloonShape);

            // 将气球对象和其运动状态保存到列表中
            _balloons.Add(new Balloon
            {
                Shape = balloonShape, X = x, Y = y, SpeedX = speedX, SpeedY = speedY, GradientStops = gradientStops
            });
        }

        // 使用CompositionTarget.Rendering事件来更新气球的位置
        CompositionTarget.Rendering += CompositionTargetBalloon_Rendering;
    }

    private void CompositionTargetBalloon_Rendering(object sender, EventArgs e)
    {
        foreach (Balloon balloon in _balloons)
        {
            // 更新气球的位置
            balloon.X += balloon.SpeedX;
            balloon.Y += balloon.SpeedY;

            // 边界检测
            if (balloon.X < 0 || balloon.X > ActualWidth - balloon.Shape.Width)
            {
                balloon.SpeedX *= -1;
            }

            if (balloon.Y < 0 || balloon.Y > ActualHeight - balloon.Shape.Height)
            {
                balloon.SpeedY *= -1;
            }

            // 更新气球的位置
            Canvas.SetLeft(balloon.Shape, balloon.X);
            Canvas.SetTop(balloon.Shape, balloon.Y);
        }
    }

    private void ClearBalloonAndBomb()
    {
        CanvasPlayground.Children.Clear();
        _balloons.Clear();
    }

    /// <summary>
    /// 重写MeasureOverride方法，引出Size参数为负数异常
    /// </summary>
    /// <param name="constraint"></param>
    /// <returns></returns>
    protected override Size MeasureOverride(Size constraint)
    {
        // 计算最后一个元素宽度，不需要关注为什么这样写，只是为了引出Size异常使得

        var lastChild = _balloons.LastOrDefault();
        if (lastChild != null)
        {
            var remainWidth = ActualWidth;
            foreach (var balloon in _balloons)
            {
                remainWidth -= balloon.Shape.Width;
            }

            lastChild.Shape.Measure(new Size(remainWidth, lastChild.Shape.Height));
        }

        return base.MeasureOverride(constraint);
    }
}