using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.MenuChange;

/// <summary>
///     MenuChange.xaml 的交互逻辑
/// </summary>
public partial class MenuChange : Window
{
    private Color[] lstColors = { Colors.Red, Colors.Blue, Colors.Green };

    public MenuChange()
    {
        InitializeComponent();
    }

    private void ButtonFechar_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ListViewMenu.SelectedIndex;
        MoveCursorMenu(index);
        switch (index)
        {
            case 0:
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new UserControlMain());
                break;
            case 1:
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new UserControlWPF());
                break;
            case 2:
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new UserControlWinform());
                break;
            case 3:
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new UserControlASPNETCORE());
                break;
            case 4:
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new UserControlXamarinForms());
                break;
            case 5:
                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(new UserControlCPP());
                break;
        }
    }

    private void MoveCursorMenu(int index)
    {
        TrainsitionigContentSlide.OnApplyTemplate();
        GridCursor.Margin = new Thickness(0, 100 + 60 * index, 0, 0);
    }
}