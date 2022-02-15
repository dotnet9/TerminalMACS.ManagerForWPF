using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.NavigationDrawMenu;

/// <summary>
///     MenuView.xaml 的交互逻辑
/// </summary>
public partial class MenuView : Window
{
    public MenuView()
    {
        InitializeComponent();
    }

    private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
    {
        // Set tooltip visibility

        if (Tg_Btn.IsChecked == true)
        {
            tt_home.Visibility = Visibility.Collapsed;
            tt_contacts.Visibility = Visibility.Collapsed;
            tt_messages.Visibility = Visibility.Collapsed;
            tt_maps.Visibility = Visibility.Collapsed;
            tt_settings.Visibility = Visibility.Collapsed;
            tt_signout.Visibility = Visibility.Collapsed;
        }
        else
        {
            tt_home.Visibility = Visibility.Visible;
            tt_contacts.Visibility = Visibility.Visible;
            tt_messages.Visibility = Visibility.Visible;
            tt_maps.Visibility = Visibility.Visible;
            tt_settings.Visibility = Visibility.Visible;
            tt_signout.Visibility = Visibility.Visible;
        }
    }

    private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
    {
        img_bg.Opacity = 1;
    }

    private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
    {
        img_bg.Opacity = 0.3;
    }

    private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Tg_Btn.IsChecked = false;
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}