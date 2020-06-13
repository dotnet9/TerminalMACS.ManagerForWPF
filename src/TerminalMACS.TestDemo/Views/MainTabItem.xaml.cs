using System.Windows;
using TerminalMACS.TestDemo.Views.FoodAppLoginUI;
using TerminalMACS.TestDemo.Views.LoginUI;
using TerminalMACS.TestDemo.Views.MobileAppUsageDashboardCore;

namespace TerminalMACS.TestDemo.Views
{
    public partial class MainTabItem
    {
        public MainTabItem()
        {
            InitializeComponent();
        }

        private void ShowFoodLoginUI_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var view= new FoodAppLoginView();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowAppUseageDashboard_Click(object sender, RoutedEventArgs e)
        {
            var view = new AppUsageDashboard();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowLoginView1_Click(object sender, RoutedEventArgs e)
        {
            var view = new LoginView1();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowLoginView2_Click(object sender, RoutedEventArgs e)
        {
            var view = new LoginView2();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }
    }
}
