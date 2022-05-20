using System.Windows;
using System.Windows.Input;
using UserGuideForMVVM.ViewModels;

namespace UserGuideForMVVM.Views;

public partial class WithGuidView : Window
{
    private readonly int _offsetLeft;
    private readonly int _offsetTop;

    public WithGuidView(int offsetLeft = 0, int offsetTop = 0)
    {
        ViewModel ??= new WithGuidViewModel();
        _offsetLeft = offsetLeft;
        _offsetTop = offsetTop;
        InitializeComponent();
    }

    public WithGuidViewModel? ViewModel
    {
        get => DataContext as WithGuidViewModel;
        set => DataContext = value;
    }

    private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}