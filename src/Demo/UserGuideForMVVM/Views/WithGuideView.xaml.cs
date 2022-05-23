using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using UserGuideForMVVM.Controls;
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

        Loaded += WithGuideView_Loaded;
    }

    public WithGuideViewModel? ViewModel
    {
        get => DataContext as WithGuideViewModel;
        set => DataContext = value;
    }

    private void WithGuideView_Loaded(object sender, RoutedEventArgs e)
    {
        GuideWindow.ShowGuideBox(new List<object> { BeautyImage });
    }

    private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            DragMove();
        }
        catch
        {
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}