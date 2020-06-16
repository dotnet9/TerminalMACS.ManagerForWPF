using System.Windows;
using TerminalMACS.TestDemo.Views.LoLGoal.Controller;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Views
{
    /// <summary>
    /// WindowProfile.xaml 的交互逻辑
    /// </summary>
    public partial class WindowProfile : Window
    {
        ControllerProfile controller;

        public WindowProfile()
        {
            controller = new ControllerProfile();
            InitializeComponent();
            this.DataContext = controller.GetContext();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            controller.OpenMain();
            this.Close();
        }
    }
}
