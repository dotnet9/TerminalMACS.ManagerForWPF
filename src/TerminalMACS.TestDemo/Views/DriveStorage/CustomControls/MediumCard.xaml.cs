using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.DriveStorage.CustomControls;

/// <summary>
///     Interaction logic for MediumCard.xaml
/// </summary>
public partial class MediumCard : UserControl
{
    // Using a DependencyProperty as the backing store for FileIcon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FileIconProperty =
        DependencyProperty.Register("FileIcon", typeof(PathGeometry), typeof(MediumCard));

    // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register("Fill", typeof(SolidColorBrush), typeof(MediumCard));

    // Using a DependencyProperty as the backing store for Text1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Text1Property =
        DependencyProperty.Register("Text1", typeof(string), typeof(MediumCard));

    // Using a DependencyProperty as the backing store for Text1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Text2Property =
        DependencyProperty.Register("Text2", typeof(string), typeof(MediumCard));

    public MediumCard()
    {
        InitializeComponent();
    }


    public PathGeometry FileIcon
    {
        get => (PathGeometry)GetValue(FileIconProperty);
        set => SetValue(FileIconProperty, value);
    }


    public SolidColorBrush Fill
    {
        get => (SolidColorBrush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }


    public string Text1
    {
        get => (string)GetValue(Text1Property);
        set => SetValue(Text1Property, value);
    }

    public string Text2
    {
        get => (string)GetValue(Text2Property);
        set => SetValue(Text2Property, value);
    }
}