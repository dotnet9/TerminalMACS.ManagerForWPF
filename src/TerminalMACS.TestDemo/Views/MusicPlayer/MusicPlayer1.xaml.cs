using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.MusicPlayer;

/// <summary>
///     MusicPlayer1.xaml 的交互逻辑
/// </summary>
public partial class MusicPlayer1 : Window
{
    public MusicPlayer1()
    {
        InitializeComponent();
    }

    private void ButtonFechar_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Proxima_Click(object sender, RoutedEventArgs e)
    {
        if (c1.Offset >= 0)
        {
            c1.Offset -= 0.01;
            c2.Offset -= 0.01;
        }
        else
        {
            c1.Offset = 1;
            c2.Offset = 0.89;
        }
    }

    private void Anterior_Click(object sender, RoutedEventArgs e)
    {
        if (c2.Offset <= 1)
        {
            c1.Offset += 0.01;
            c2.Offset += 0.01;
        }
        else
        {
            c1.Offset = 0.11;
            c2.Offset = 0;
        }
    }

    private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}