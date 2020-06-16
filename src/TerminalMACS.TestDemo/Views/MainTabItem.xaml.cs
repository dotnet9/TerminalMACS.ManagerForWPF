using System.Windows;
using TerminalMACS.TestDemo.Views.AnimatedMenu;
using TerminalMACS.TestDemo.Views.FoodAppLoginUI;
using TerminalMACS.TestDemo.Views.InstagramRedesign;
using TerminalMACS.TestDemo.Views.LoginUI;
using TerminalMACS.TestDemo.Views.MenuChange;
using TerminalMACS.TestDemo.Views.MobileAppUsageDashboardCore;
using TerminalMACS.TestDemo.Views.MusicPlayer;

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

        private void ShowMusicPalyer1_Click(object sender, RoutedEventArgs e)
        {
            var view = new MusicPlayer1();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowDashboard2_Click(object sender, RoutedEventArgs e)
        {
            var view = new Dashboard2();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowMenuChange_Click(object sender, RoutedEventArgs e)
        {
            var view = new MenuChange.MenuChange();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowAnimatedMenu_Click(object sender, RoutedEventArgs e)
        {
            var view = new AnimatedMenuView();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }

        private void ShowInstagramRedesign_Click(object sender, RoutedEventArgs e)
        {
            var view = new InstagramRedesignView();
            view.Owner = Application.Current.MainWindow;
            view.Show();
        }
    }
}
