using System.Windows;

namespace Dotnet9Playground;

/// <summary>
///     综合小案例:模拟.NET应用场景，综合应用反编译、第三方库调试、拦截、一库多版本兼容
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartGame_OnClick(object sender, RoutedEventArgs e)
    {
        MyBallGame.StartGame();
    }
}