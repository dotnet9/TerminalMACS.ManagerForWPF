using System.Windows;

namespace SimpleGuide;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowNormalWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuidWindow(20, 40)
        {
            Title = "有边框窗体测试引导"
        };
        window.Show();
    }

    private void ShowWithoutBorderWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuidWindow
        {
            Title = "无边框窗体测试引导",
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None
        };
        window.Show();
    }
}