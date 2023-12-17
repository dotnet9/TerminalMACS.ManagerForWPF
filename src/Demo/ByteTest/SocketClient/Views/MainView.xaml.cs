using System.Windows;
using SocketClient.ViewModels;

namespace SocketClient;

public partial class MainView : Window
{
    public MainViewModel? ViewModel
    {
        get => DataContext as MainViewModel;
        set => DataContext = value;
    }

    public MainView()
    {
        ViewModel = new MainViewModel { Owner = this };
        InitializeComponent();
    }
}