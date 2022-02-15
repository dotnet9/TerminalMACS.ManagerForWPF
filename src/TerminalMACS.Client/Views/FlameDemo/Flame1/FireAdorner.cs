using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TerminalMACS.Client.Views.FlameDemo.Flame1;

/// <summary>
///     Adorner that disables all controls that fall under it
/// </summary>
public class FireAdorner : Adorner
{
    private const int DPI = 96;
    private readonly FireGenerator _fireGenerator = new(600, 50);
    private BitmapPalette _pallette;

    /// <summary>
    ///     Constructor for the adorner
    /// </summary>
    /// <param name="adornerElement">The element to be adorned</param>
    public FireAdorner(UIElement adornerElement)
        : base(adornerElement)
    {
        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
        InvalidateVisual();
    }

    /// <summary>
    ///     Called to draw on screen
    /// </summary>
    /// <param name="drawingContext">The drawind context in which we can draw</param>
    protected override void OnRender(DrawingContext drawingContext)
    {
        // only set the pallette once (dont do in constructor as causes odd errors if exception occurs)

        _pallette = SetupFirePalette();

        _fireGenerator.UpdateFire();

        var bs = BitmapSource.Create(_fireGenerator.Width, _fireGenerator.Height, DPI, DPI,
            PixelFormats.Indexed8, _pallette, _fireGenerator.FireData, _fireGenerator.Width);
        drawingContext.DrawImage(bs, new Rect(0, 0, DesiredSize.Width, DesiredSize.Height));
    }

    private BitmapPalette SetupFirePalette()
    {
        var myList = new List<Color>();

        // seutp the basic array we will modify
        for (var i = 0; i <= 255; i++) myList.Add(new Color());

        for (var i = 0; i < 64; i++)
        {
            var c1 = new Color();
            c1.R = (byte)(i * 4);
            c1.G = 0;
            c1.B = 0;
            c1.A = 255;
            myList[i] = c1;

            var c2 = new Color();
            c2.R = 255;
            c2.G = (byte)(i * 4);
            c2.B = 0;
            c2.A = 255;
            myList[i + 64] = c2;

            var c3 = new Color();
            c3.R = 255;
            c3.G = 255;
            c3.B = (byte)(i * 4);
            c3.A = 255;
            myList[i + 128] = c3;

            var c4 = new Color();
            c4.R = 255;
            c4.G = 255;
            c4.B = 255;
            c4.A = 255;
            myList[i + 192] = c4;
        }

        var bp = new BitmapPalette(myList);
        return bp;
    }
}