namespace SkiaSharpDemo;

public class FiveRings
{
    public SKPoint centerPoint;
    public int Radius = 0;
    public int BallLength = 8;

    public double TargetX;
    public double Spring = 0.03;
    public double SpringLength = 200;
    public double Friction = 0.95;
    public List<Ball>? Balls;
    public Ball? draggedBall;

    public void init(SKCanvas canvas, SKTypeface Font, int Width, int Height)
    {
        if (Balls == null)
        {
            Balls = new List<Ball>();
            for (int i = 0; i < BallLength; i++)
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                Balls.Add(new Ball()
                {
                    X = random.Next(50, Width - 50),
                    Y = random.Next(50, Height - 50),
                    Radius = this.Radius
                });
            }
        }
    }

    /// <summary>
    /// 渲染
    /// </summary>
    public void Render(SKCanvas canvas, SKTypeface Font, int Width, int Height)
    {
        centerPoint = new SKPoint(Width / 2, Height / 2);
        this.Radius = 20;
        this.TargetX = Width / 2;
        init(canvas, Font, Width, Height);
        canvas.Clear(SKColors.White);


        //划线
        using var LinePaint = new SKPaint
        {
            Color = SKColors.Green,
            Style = SKPaintStyle.Fill,
            StrokeWidth = 3,
            IsStroke = true,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };
        SKPath path = null;
        foreach (var item in Balls)
        {
            if (path == null)
            {
                path = new SKPath();
                path.MoveTo((float)item.X, (float)item.Y);
            }
            else
            {
                path.LineTo((float)item.X, (float)item.Y);
            }
        }

        path.Close();
        canvas.DrawPath(path, LinePaint);


        foreach (var item in Balls)
        {
            if (!item.Dragged)
            {
                foreach (var ball in Balls.Where(t => t != item).ToList())
                {
                    SpringTo(item, ball);
                }
            }

            DrawCircle(canvas, item);
        }

        using var paint = new SKPaint
        {
            Color = SKColors.Blue,
            IsAntialias = true,
            Typeface = Font,
            TextSize = 24
        };
        string by = $"by 蓝创精英团队";
        canvas.DrawText(by, 600, 400, paint);
    }

    /// <summary>
    /// 画一个圆
    /// </summary>
    public void DrawCircle(SKCanvas canvas, Ball ball)
    {
        using var paint = new SKPaint
        {
            Color = SKColors.Blue,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            StrokeWidth = 2
        };
        canvas.DrawCircle((float)ball.X, (float)ball.Y, ball.Radius, paint);
    }

    public void MouseMove(SKPoint sKPoint)
    {
        if (draggedBall != null)
        {
            draggedBall.X = sKPoint.X;
            draggedBall.Y = sKPoint.Y;
        }
    }

    public void MouseDown(SKPoint sKPoint)
    {
        foreach (var item in Balls)
        {
            if (item.CheckPoint(sKPoint))
            {
                item.Dragged = true;
                draggedBall = item;
            }
            else
            {
                item.Dragged = false;
            }
        }
    }

    public void MouseUp(SKPoint sKPoint)
    {
        draggedBall = null;
        foreach (var item in Balls)
        {
            item.Dragged = false;
        }
    }

    public void SpringTo(Ball b1, Ball b2)
    {
        var dx = b2.X - b1.X;
        var dy = b2.Y - b1.Y;
        var angle = Math.Atan2(dy, dx);
        var targetX = b2.X - SpringLength * Math.Cos(angle);
        var targetY = b2.Y - SpringLength * Math.Sin(angle);

        b1.VX += (targetX - b1.X) * Spring;
        b1.VY += (targetY - b1.Y) * Spring;

        b1.VX *= Friction;
        b1.VY *= Friction;

        b1.X += b1.VX;
        b1.Y += b1.VY;
    }
}