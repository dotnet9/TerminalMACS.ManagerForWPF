using System.Windows;

namespace HookWpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowDialog_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("https://dotnet9.com 是一个热衷于技术分享的程序员网站", "Dotnet9");
    }

    private void ShowBadMessageDialog_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("这是一个垃圾网站", "https://dotnet9.com");
    }
}