using System.Windows;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut.Views;

public partial class TestComboBoxWithDataGridView : Window
{
    public TestComboBoxWithDataGridViewModel? ViewModel
    {
        get => DataContext as TestComboBoxWithDataGridViewModel;
        set => DataContext = value;
    }

    public TestComboBoxWithDataGridView()
    {
        ViewModel = new TestComboBoxWithDataGridViewModel();
        InitializeComponent();
    }
}