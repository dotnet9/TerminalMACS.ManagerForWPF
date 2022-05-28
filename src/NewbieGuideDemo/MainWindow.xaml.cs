using System.Windows;
using System.Windows.Input;

namespace NewbieGuideDemo
{
    /// <summary>
    /// 测试 Dotnet9WPFControls新手引导功能
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
