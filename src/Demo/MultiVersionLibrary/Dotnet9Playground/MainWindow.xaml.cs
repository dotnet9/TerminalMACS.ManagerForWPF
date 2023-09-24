using System.Windows;

namespace Dotnet9Playground;

/// <summary>
///     MainWindow.xaml 的交互逻辑
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