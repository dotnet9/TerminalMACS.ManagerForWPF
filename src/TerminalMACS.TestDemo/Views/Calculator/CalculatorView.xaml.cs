using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.Calculator;

/// <summary>
///     CalculatorView.xaml 的交互逻辑
/// </summary>
public partial class CalculatorView : Window
{
    public CalculatorView()
    {
        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}