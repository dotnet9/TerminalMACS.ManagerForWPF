using System.Windows;
using SocketServer.ViewModels;

namespace SocketServer;

public partial class MainView : Window
{
    public MainViewModel? ViewModel
    {
        get => DataContext as MainViewModel;
        set => DataContext = value;
    }

    public MainView()
    {
        ViewModel = new MainViewModel();
        InitializeComponent();
    }
}