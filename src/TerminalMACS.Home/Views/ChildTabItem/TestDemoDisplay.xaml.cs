using System;
using System.IO;
using System.Windows.Controls;

namespace TerminalMACS.Home.Views.ChildTabItem;

/// <summary>
///     TestDemoDisplay.xaml 的交互逻辑
/// </summary>
public partial class TestDemoDisplay : UserControl
{
    public TestDemoDisplay()
    {
        InitializeComponent();

        var imgs = Directory.GetFiles("./../../../assets/TestDemos");
        if (imgs.Length > 0)
            foreach (var imgPath in imgs)
            {
                var fullPath = Path.GetFullPath(imgPath);
                CoverFlowMain.Add(new Uri(fullPath, UriKind.RelativeOrAbsolute));
            }

        CoverFlowMain.PageIndex = 2;
    }
}