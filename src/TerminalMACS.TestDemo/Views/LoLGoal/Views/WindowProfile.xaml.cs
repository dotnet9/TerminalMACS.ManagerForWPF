using System.Windows;
using TerminalMACS.TestDemo.Views.LoLGoal.Controller;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Views;

/// <summary>
///     WindowProfile.xaml 的交互逻辑
/// </summary>
public partial class WindowProfile : Window
{
    private readonly ControllerProfile controller;

    public WindowProfile()
    {
        controller = new ControllerProfile();
        InitializeComponent();
        DataContext = controller.GetContext();
    }

    private void ButtonSearch_Click(object sender, RoutedEventArgs e)
    {
        controller.OpenMain();
        Close();
    }
}