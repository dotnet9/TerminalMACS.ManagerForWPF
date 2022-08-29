using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut.Views;

/// <summary>
///     Interaction logic for TestWindows.xaml
/// </summary>
public partial class TestWindows : Window
{
    private bool _isPress;

    public TestWindows()
    {
        ViewModel = new TestWindowsViewModel();
        InitializeComponent();
    }

    public TestWindowsViewModel? ViewModel
    {
        get => DataContext as TestWindowsViewModel;
        set => DataContext = value;
    }

    private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isPress = true;
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(150));
            if (!_isPress) Dispatcher.Invoke(() => { ChangeTime_Click(sender, e); });

            Debug.Print(_isPress ? "长按" : "短按，模拟点击");
        });
    }

    private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isPress = false;
        Debug.Print("UIElement_OnPreviewMouseLeftButtonUp");
    }

    private void ChangeTime_Click(object sender, RoutedEventArgs e)
    {
        Title = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

    private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isPress = false;
        Debug.Print("UIElement_OnMouseLeftButtonUp");
    }
}