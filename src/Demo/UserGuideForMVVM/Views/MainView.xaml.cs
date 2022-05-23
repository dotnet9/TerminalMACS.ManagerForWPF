using System.Windows;
using UserGuideForMVVM.Controls;
using UserGuideForMVVM.ViewModels;

namespace UserGuideForMVVM.Views;

public partial class MainView : Window
{
    public MainView()
    {
        ViewModel ??= new MainViewModel();
        InitializeComponent();
    }

    public MainViewModel? ViewModel
    {
        get => DataContext as MainViewModel;
        set => DataContext = value;
    }

    private void ShowNormalWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuideView(20, 40)
        {
            Title = "有边框窗体测试引导"
        };
        window.Show();
    }

    private void ShowWithoutBorderWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuideView
        {
            Title = "无边框窗体测试引导",
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None
        };
        window.Show();
    }
}