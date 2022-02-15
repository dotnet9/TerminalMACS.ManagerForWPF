using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.LoginUI;

/// <summary>
///     LoginView2.xaml 的交互逻辑
/// </summary>
public partial class LoginView2 : Window
{
    public LoginView2()
    {
        InitializeComponent();
    }

    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}