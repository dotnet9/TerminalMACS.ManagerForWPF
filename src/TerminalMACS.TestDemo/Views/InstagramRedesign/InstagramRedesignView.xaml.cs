using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.InstagramRedesign
{
    /// <summary>
    /// InstagramRedesignView.xaml 的交互逻辑
    /// </summary>
    public partial class InstagramRedesignView : Window
    {
        public InstagramRedesignView()
        {
            InitializeComponent();
        }
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
