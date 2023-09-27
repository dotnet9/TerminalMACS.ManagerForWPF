using Dotnet9Games.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

    private readonly List<BallInfo> _balloons = new();

    private readonly Random _random = new(DateTime.Now.Millisecond);
    private int _allScore;


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
            // 生成彩色气球控件、位置和速度
            BallInfo ballInfo = new BallInfo()
            {
                X = _random.Next((int)ActualWidth - 50),
                Y = _random.Next((int)ActualHeight - 50),
                SpeedX = _random.NextDouble() * 2 - 1,
                SpeedY = _random.NextDouble() * 2 - 1
            };
            ballInfo.Ball = new Ball() { Tag = ballInfo };
            ballInfo.Ball.MouseLeftButtonDown += Ball_MouseLeftButtonDown;

            // 将气球添加到用户控件上的某个容器中
            CanvasPlayground.Children.Add(ballInfo.Ball);

            // 将气球对象和其运动状态保存到列表中
            _balloons.Add(ballInfo);
        }

        // 使用CompositionTarget.Rendering事件来更新气球的位置
        CompositionTarget.Rendering += CompositionTargetBalloon_Rendering;
    }

    /// <summary>
    /// 点击气球，执行删除和积分动作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Ball_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is not Ball ball)
        {
            return;
        }

        PlaySound();
        var ballInfo = (BallInfo)ball.Tag;
        _allScore += ball.Score;
        CanvasPlayground.Children.Remove(ball);
        _balloons.Remove(ballInfo!);
        RunAllScore.Text = $"{_balloons.Count}/{BallCount}, {_allScore}";
    }

    private void PlaySound()
    {
        Task.Run(async () =>
        {
            var player = new MediaPlayer();

            var file = Guid.NewGuid().ToString() + ".mp3";
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(Resource.气球爆裂声, 0, Resource.气球爆裂声.Length);
            }

            player.Open(new Uri(file, UriKind.Relative));
            player.Play();
            await Task.Delay(TimeSpan.FromMilliseconds(3000));
            File.Delete(file);
        });
    }

    private void CompositionTargetBalloon_Rendering(object sender, EventArgs e)
    {
        foreach (BallInfo balloon in _balloons)
        {
            // 更新气球的位置
            balloon.X += balloon.SpeedX;
            balloon.Y += balloon.SpeedY;

            // 边界检测
            if (balloon.X < 0 || balloon.X > ActualWidth - balloon.Ball.Width)
            {
                balloon.SpeedX *= -1;
            }

            if (balloon.Y < 0 || balloon.Y > ActualHeight - balloon.Ball.Height)
            {
                balloon.SpeedY *= -1;
            }

            // 更新气球的位置
            Canvas.SetLeft(balloon.Ball, balloon.X);
            Canvas.SetTop(balloon.Ball, balloon.Y);
        }
    }

    private void ClearBalloonAndBomb()
    {
        CanvasPlayground.Children.Clear();
        _balloons.Clear();
        _allScore = 0;
        RunAllScore.Text = $"{_allScore}";
    }

    /// <summary>
    /// 重写MeasureOverride方法，引出Size参数为负数异常
    /// </summary>
    /// <param name="constraint"></param>
    /// <returns></returns>
    protected override Size MeasureOverride(Size constraint)
    {
        // 计算最后一个元素宽度，不需要关注为什么这样写，只是为了引出Size异常使得

        //var lastChild = _balloons.LastOrDefault();
        //if (lastChild != null)
        //{
        //    var remainWidth = ActualWidth;
        //    foreach (var balloon in _balloons)
        //    {
        //        remainWidth -= balloon.Ball.Width;
        //    }

        //    lastChild.Ball.Measure(new Size(remainWidth, lastChild.Ball.Height));
        //}

        return base.MeasureOverride(constraint);
    }
}