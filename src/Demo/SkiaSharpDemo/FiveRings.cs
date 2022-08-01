namespace SkiaSharpDemo;

public class FiveRings
{
    public int BallLength = 8;
    public List<Ball>? Balls;
    public SKPoint CenterPoint;
    public Ball? DraggedBall;
    public double Friction = 0.95;
    public int Radius;
    public double Spring = 0.03;
    public double SpringLength = 200;

    public double TargetX;

    public void Init(SKCanvas canvas, SKTypeface font, int width, int height)
    {
        if (Balls != null) return;
        Balls = new List<Ball>();
        for (var i = 0; i < BallLength; i++)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            Balls.Add(new Ball
            {
                X = random.Next(50, width - 50),
                Y = random.Next(50, height - 50),
                Radius = Radius
            });
        }
    }

    /// <summary>
    ///     渲染
    /// </summary>
    public void Render(SKCanvas canvas, SKTypeface font, int width, int height)
    {
        CenterPoint = new SKPoint(width / 2, height / 2);
        Radius = 20;
        TargetX = width / 2;
        Init(canvas, font, width, height);
        canvas.Clear(SKColors.White);


        //划线
        using var linePaint = new SKPaint
        {
            Color = SKColors.Green,
            Style = SKPaintStyle.Fill,
            StrokeWidth = 3,
            IsStroke = true,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };
        SKPath? path = null;
        foreach (var item in Balls!)
            if (path == null)
            {
                path = new SKPath();
                path.MoveTo((float)item.X, (float)item.Y);
            }
            else
            {
                path.LineTo((float)item.X, (float)item.Y);
            }

        path!.Close();
        canvas.DrawPath(path, linePaint);


        foreach (var item in Balls)
        {
            if (!item.Dragged)
                foreach (var ball in Balls.Where(t => t != item).ToList())
                    SpringTo(item, ball);

            DrawCircle(canvas, item);
        }

        using var paint = new SKPaint
        {
            Color = SKColors.Blue,
            IsAntialias = true,
            Typeface = font,
            TextSize = 24
        };
        const string by = "by 蓝创精英团队";
        canvas.DrawText(by, 600, 400, paint);
    }

    /// <summary>
    ///     画一个圆
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
        if (DraggedBall == null) return;
        DraggedBall.X = sKPoint.X;
        DraggedBall.Y = sKPoint.Y;
    }

    public void MouseDown(SKPoint sKPoint)
    {
        foreach (var item in Balls)
            if (item.CheckPoint(sKPoint))
            {
                item.Dragged = true;
                DraggedBall = item;
            }
            else
            {
                item.Dragged = false;
            }
    }

    public void MouseUp(SKPoint sKPoint)
    {
        DraggedBall = null;
        foreach (var item in Balls) item.Dragged = false;
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