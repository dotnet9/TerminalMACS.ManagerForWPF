using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.MobileAppUsageDashboardCore
{
    /// <summary>
    /// Dashboard2.xaml 的交互逻辑
    /// </summary>
    public partial class Dashboard2 : Window
    {
        public Dashboard2()
        {
            InitializeComponent();
            var echarHtmlFile = $"{AppDomain.CurrentDomain.BaseDirectory}pie-doughnut.html";
            if (System.IO.File.Exists(echarHtmlFile))
            {
                this.web.Navigate(echarHtmlFile);
            }
            else
            {
                MessageBox.Show($"Please copy pie-doughnut.html to output dir:{echarHtmlFile }");
            }
        }
        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void GridBarraTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}