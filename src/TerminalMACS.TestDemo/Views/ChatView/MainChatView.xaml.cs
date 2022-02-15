using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.ChatView;

/// <summary>
///     MainChatView.xaml 的交互逻辑
/// </summary>
public partial class MainChatView : Window
{
    public MainChatView()
    {
        InitializeComponent();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}