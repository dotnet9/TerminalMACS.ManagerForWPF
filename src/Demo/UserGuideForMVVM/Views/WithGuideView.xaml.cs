using System.Windows;
using System.Windows.Input;
using UserGuideForMVVM.ViewModels;

namespace UserGuideForMVVM.Views;

public partial class WithGuideView : Window
{
    private readonly int _offsetLeft;
    private readonly int _offsetTop;

    public WithGuideView(int offsetLeft = 0, int offsetTop = 0)
    {
        ViewModel ??= new WithGuideViewModel();
        _offsetLeft = offsetLeft;
        _offsetTop = offsetTop;
        InitializeComponent();
    }

    public WithGuideViewModel? ViewModel
    {
        get => DataContext as WithGuideViewModel;
        set => DataContext = value;
    }

    private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}