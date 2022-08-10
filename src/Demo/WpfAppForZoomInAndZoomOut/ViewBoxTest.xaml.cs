using System.Windows;
using System.Windows.Controls;

namespace WpfAppForZoomInAndZoomOut;

/// <summary>
///     Interaction logic for ViewBoxTest.xaml
/// </summary>
public partial class ViewBoxTest : UserControl
{
    public ViewBoxTest()
    {
        InitializeComponent();
    }

    private void ViewBoxTest_OnLoaded(object sender, RoutedEventArgs e)
    {
        ChildViewIns1.Width = ActualWidth;
        ChildViewIns1.Height = ActualHeight;
    }
}