using RibbonDockSample.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RibbonDockSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FluentRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FluentRibbonWindow();
            window.Show();
        }

        private void AvalonDockButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AvalonDockWindow();
            window.Show();
        }

        private void FluentRibbonAndAvalonDockButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FluentRibbonAndAvalonDockWindow();
            window.Show();
        }

        private void ActiproRibbonDockButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActiproRibbonDockWindow();
            window.Show();
        }
    }
}
