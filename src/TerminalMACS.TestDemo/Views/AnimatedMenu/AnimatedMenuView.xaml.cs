using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.AnimatedMenu;

/// <summary>
///     AnimatedMenuView.xaml 的交互逻辑
/// </summary>
public partial class AnimatedMenuView : Window
{
    public AnimatedMenuView()
    {
        InitializeComponent();
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
    {
        ButtonOpenMenu.Visibility = Visibility.Collapsed;
        ButtonCloseMenu.Visibility = Visibility.Visible;
    }

    private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
    {
        ButtonOpenMenu.Visibility = Visibility.Visible;
        ButtonCloseMenu.Visibility = Visibility.Collapsed;
    }

    private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}