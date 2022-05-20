using System.Windows;

namespace UserGuideForMVVM.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private void ShowNormalWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuidView(20, 40)
        {
            Title = "有边框窗体测试引导"
        };
        window.Show();
    }

    private void ShowWithoutBorderWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuidView
        {
            Title = "无边框窗体测试引导",
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None
        };
        window.Show();
    }
}