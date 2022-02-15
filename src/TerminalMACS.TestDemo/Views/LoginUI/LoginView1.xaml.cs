using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.LoginUI;

/// <summary>
///     LoginView1.xaml 的交互逻辑
/// </summary>
public partial class LoginView1 : Window
{
    public LoginView1()
    {
        InitializeComponent();
    }

    private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}