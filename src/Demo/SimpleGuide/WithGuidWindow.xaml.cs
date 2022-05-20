using System.Collections.Generic;
using System.Windows;

namespace SimpleGuide;

public partial class WithGuidWindow : Window
{
    private readonly int _offsetLeft;
    private readonly int _offsetTop;

    public WithGuidWindow(int offsetLeft = 0, int offsetTop = 0)
    {
        _offsetLeft = offsetLeft;
        _offsetTop = offsetTop;
        InitializeComponent();
        Loaded += (sender, args) => ShowGuide_Click(null, null);
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

        var win = new GuideWin(this, list, _offsetLeft, _offsetTop);

        win.ShowDialog();
    }

    private void DragMove_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.DragMove();
    }
}