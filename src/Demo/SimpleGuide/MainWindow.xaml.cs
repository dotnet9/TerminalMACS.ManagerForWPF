using System.Collections.Generic;
using System.Windows;

namespace SimpleGuide;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (sender, args) => ShowGuide_Click(null,null);
    }

    private void ShowGuide_Click(object sender, RoutedEventArgs e)
    {
        var list = new List<GuidVo>()
        {
            new GuidVo()
            {
                Uc = imgPublic1,
                Content = "第一步：关注 Dotnet9 公众号"
            },
            new GuidVo()
            {
                Uc = imgPublic2,
                Content = "第二步：关注 快乐玩转技术 公众号"
            },
            new GuidVo()
            {
                Uc = imgOwner,
                Content = "第三步：联系我"
            }
        };

        var win = new GuideWin(this, list);

        win.ShowDialog();
    }
}