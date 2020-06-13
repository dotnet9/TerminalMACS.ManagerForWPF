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

namespace TerminalMACS.TestDemo.Views.LoginUI
{
    /// <summary>
    /// LoginView2.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView2 : Window
    {
        public LoginView2()
        {
            InitializeComponent();
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
