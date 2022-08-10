using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfAppForZoomInAndZoomOut;

public partial class ScaleTransformTest : UserControl
{
    public ScaleTransformTest()
    {
        InitializeComponent();
    }


    private void Button80_Click(object sender, RoutedEventArgs e)
    {
        Scale(0.8);
    }

    private void Scale(double scale)
    {
        //this.st.CenterX = this.st.CenterY = this.st.ScaleX = this.st.ScaleY = scale;
        sliderForScale.Value = scale;
        ZoomParentBorder.Width = ZoomScrollViewer.ActualWidth * scale;
        ZoomParentBorder.Height = ZoomScrollViewer.ActualHeight * scale;
    }


    private void Button100_Click(object sender, RoutedEventArgs e)
    {
        Scale(1);
    }

    private void Button120_Click(object sender, RoutedEventArgs e)
    {
        Scale(1.2);
    }

    private void Button150_Click(object sender, RoutedEventArgs e)
    {
        Scale(1.5);
    }

    private void Button200_Click(object sender, RoutedEventArgs e)
    {
        Scale(2);
    }

    private void Button300_Click(object sender, RoutedEventArgs e)
    {
        Scale(3);
    }

    private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control) return;

        var slideValue = sliderForScale.Value;
        if (e.Delta > 0)
            slideValue += 0.2;
        else
            slideValue -= 0.2;

        if (slideValue < sliderForScale.Minimum) slideValue = sliderForScale.Minimum;

        if (slideValue > sliderForScale.Maximum) slideValue = sliderForScale.Maximum;

        Scale(slideValue);
    }
}