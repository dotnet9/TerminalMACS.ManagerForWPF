using System.Windows;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut.Views;

public partial class TestUserControlCommandWindow : Window
{
    public TestUserControlCommandWindow()
    {
        ViewModel = new TestUserControlCommandWindowViewModel();
        InitializeComponent();
    }

    public TestUserControlCommandWindowViewModel? ViewModel
    {
        get => DataContext as TestUserControlCommandWindowViewModel;
        set => DataContext = value;
    }
}