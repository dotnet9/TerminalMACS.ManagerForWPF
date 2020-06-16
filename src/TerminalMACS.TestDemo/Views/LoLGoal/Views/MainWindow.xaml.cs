using System.Windows;
using TerminalMACS.TestDemo.Views.LoLGoal.Controller;
using TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ControllerMain controller;
        ViewModelMain viewModel;
        public MainWindow()
        {
            controller = new ControllerMain();
            viewModel = new ViewModelMain();
            InitializeComponent();

            this.DataContext = viewModel;
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Region))
                return;
            if (string.IsNullOrEmpty(viewModel.SummonerName))
                return;

            if (controller.GetSummoner(viewModel.SummonerName))
            {
                WindowProfile profile = new WindowProfile();
                profile.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Not Found");
            }
        }
    }
}
