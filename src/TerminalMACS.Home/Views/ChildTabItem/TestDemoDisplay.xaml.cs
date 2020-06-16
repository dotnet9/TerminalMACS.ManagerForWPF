using System;
using System.Windows.Controls;
using System.Linq;
using System.Threading;

namespace TerminalMACS.Home.Views.ChildTabItem
{
    /// <summary>
    /// TestDemoDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class TestDemoDisplay : UserControl
    {
        public TestDemoDisplay()
        {
            InitializeComponent();

            var imgs = System.IO.Directory.GetFiles("./../../../assets/TestDemos");
            if (imgs.Length > 0)
            {
                foreach (var imgPath in imgs)
                {
                    var fullPath = System.IO.Path.GetFullPath(imgPath);
                    CoverFlowMain.Add(new Uri(fullPath, UriKind.RelativeOrAbsolute));
                }
            }
            CoverFlowMain.JumpTo(2);
        }
    }
}
