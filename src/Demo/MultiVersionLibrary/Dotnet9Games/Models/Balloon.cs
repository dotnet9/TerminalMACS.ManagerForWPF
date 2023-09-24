using System.Windows.Media;
using System.Windows.Shapes;

namespace Dotnet9Games.Models;

internal class Balloon
{
    public Ellipse Shape { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double SpeedX { get; set; }
    public double SpeedY { get; set; }
    public Color Color { get; set; }
    public GradientStopCollection GradientStops { get; set; }
}