using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.DriveStorage.CustomControls;

/// <summary>
///     Interaction logic for CircularProgressBar.xaml
/// </summary>
public partial class CircularProgressBar : UserControl
{
    // Using a DependencyProperty as the backing store for IndicatorBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IndicatorBrushProperty =
        DependencyProperty.Register("IndicatorBrush", typeof(Brush), typeof(CircularProgressBar));

    // Using a DependencyProperty as the backing store for BackgroundBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BackgroundBrushProperty =
        DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(CircularProgressBar));

    // Using a DependencyProperty as the backing store for BackgroundBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PFontColorProperty =
        DependencyProperty.Register("PFontColor", typeof(Brush), typeof(CircularProgressBar));

    // Using a DependencyProperty as the backing store for BackgroundBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PFontWeightProperty =
        DependencyProperty.Register("PFontWeight", typeof(FontWeight), typeof(CircularProgressBar));

    // Using a DependencyProperty as the backing store for ArcThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ArcThicknessProperty =
        DependencyProperty.Register("ArcThickness", typeof(int), typeof(CircularProgressBar));

    // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(int), typeof(CircularProgressBar));

    public CircularProgressBar()
    {
        InitializeComponent();
    }

    public Brush IndicatorBrush
    {
        get => (Brush)GetValue(IndicatorBrushProperty);
        set => SetValue(IndicatorBrushProperty, value);
    }

    public Brush BackgroundBrush
    {
        get => (Brush)GetValue(BackgroundBrushProperty);
        set => SetValue(BackgroundBrushProperty, value);
    }

    public Brush PFontColor
    {
        get => (Brush)GetValue(PFontColorProperty);
        set => SetValue(PFontColorProperty, value);
    }

    public FontWeight PFontWeight
    {
        get => (FontWeight)GetValue(PFontWeightProperty);
        set => SetValue(PFontWeightProperty, value);
    }

    public int ArcThickness
    {
        get => (int)GetValue(ArcThicknessProperty);
        set => SetValue(ArcThicknessProperty, value);
    }


    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}

[ValueConversion(typeof(int), typeof(double))]
public class ValueToAngleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value * 0.01 * 360;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)((double)value * 360) * 100;
    }
}

public class ValueToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Format("{0}{1}", value, "");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}