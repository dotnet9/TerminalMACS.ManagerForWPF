using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfAppForZoomInAndZoomOut.Views;


public partial class TestUserControlCommandView : UserControl {
    public ICommand? TestCommand
    {
        get => (ICommand?)GetValue(TestCommandProperty);
        set=> SetValue(TestCommandProperty, value);
    }
    public static readonly DependencyProperty TestCommandProperty=DependencyProperty.Register(nameof(TestCommand),typeof(ICommand),typeof(TestUserControlCommandView));

    public TestUserControlCommandView() {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        TestCommand?.Execute(null);
    }
}
