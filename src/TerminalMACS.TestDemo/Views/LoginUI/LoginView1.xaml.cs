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
    /// LoginView1.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView1 : Window
    {
        public LoginView1()
        {
            InitializeComponent();
        }
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
