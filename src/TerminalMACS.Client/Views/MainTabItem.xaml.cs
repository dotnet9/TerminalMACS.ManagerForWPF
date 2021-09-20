using System.Windows.Documents;
using TerminalMACS.Client.Views.FlameDemo.Flame1;

namespace TerminalMACS.Client.Views
{
    public partial class MainTabItem
    {
        public MainTabItem()
        {
            InitializeComponent();
        }

        private bool isLoad = false;
        // 参考链接：https://ask.csdn.net/questions/271708
        private void ShowFlame1_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            if (!isLoad)
            {
                isLoad = true;

				AdornerLayer.GetAdornerLayer(this.spFlamePanel).Add(new FireAdorner(this.spFlamePanel));
            }
		}
	}
}
