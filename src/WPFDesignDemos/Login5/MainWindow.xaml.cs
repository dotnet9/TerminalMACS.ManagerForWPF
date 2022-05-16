using System.Windows;
using System.Windows.Input;

namespace Login5;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }

    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}