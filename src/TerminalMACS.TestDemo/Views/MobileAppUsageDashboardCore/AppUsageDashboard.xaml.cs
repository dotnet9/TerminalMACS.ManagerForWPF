using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TerminalMACS.TestDemo.Views.MobileAppUsageDashboardCore
{
    /// <summary>
    /// AppUsageDashboard.xaml 的交互逻辑
    /// </summary>
    public partial class AppUsageDashboard : Window
    {
        public AppUsageDashboard()
        {
            InitializeComponent();
        }
        private void todayBtnClicked(object sender, RoutedEventArgs e)
        {
            mainDrawer.IsLeftDrawerOpen = false;
            todayRadio.IsChecked = true;
            monthRadio.IsChecked = false;
            weekRadio.IsChecked = false;
        }

        private void weekBtnClicked(object sender, RoutedEventArgs e)
        {
            mainDrawer.IsLeftDrawerOpen = false;
            todayRadio.IsChecked = false;
            weekRadio.IsChecked = true;
            monthRadio.IsChecked = false;
        }

        private void monthBtnClicked(object sender, RoutedEventArgs e)
        {
            mainDrawer.IsLeftDrawerOpen = false;
            todayRadio.IsChecked = false;
            weekRadio.IsChecked = false;
            monthRadio.IsChecked = true;
        }

        private void dragME(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

                //throw;
            }
        }
    }
}
