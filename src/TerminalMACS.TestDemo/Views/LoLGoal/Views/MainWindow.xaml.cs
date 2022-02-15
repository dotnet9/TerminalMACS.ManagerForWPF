using System.Windows;
using TerminalMACS.TestDemo.Views.LoLGoal.Controller;
using TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Views;

/// <summary>
///     MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    private readonly ControllerMain controller;
    private readonly ViewModelMain viewModel;

    public MainWindow()
    {
        controller = new ControllerMain();
        viewModel = new ViewModelMain();
        InitializeComponent();

        DataContext = viewModel;
    }

    private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(viewModel.Region))
            return;
        if (string.IsNullOrEmpty(viewModel.SummonerName))
            return;

        if (controller.GetSummoner(viewModel.SummonerName))
        {
            var profile = new WindowProfile();
            profile.Show();
            Close();
        }
        else
        {
            MessageBox.Show("Not Found");
        }
    }
}