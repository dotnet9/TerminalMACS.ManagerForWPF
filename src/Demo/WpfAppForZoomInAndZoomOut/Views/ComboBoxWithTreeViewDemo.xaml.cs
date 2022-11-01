using System.Windows;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut.Views;

/// <summary>
///     ComboBoxWithTreeViewDemo.xaml 的交互逻辑
/// </summary>
public partial class ComboBoxWithTreeViewDemo : Window
{
    public ComboBoxWithTreeViewDemo()
    {
        ViewModel = new ComboBoxWithTreeViewDemoViewModel();
        InitializeComponent();
    }

    public ComboBoxWithTreeViewDemoViewModel? ViewModel
    {
        get => DataContext as ComboBoxWithTreeViewDemoViewModel;
        set => DataContext = value;
    }
}