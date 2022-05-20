using System.Collections.Generic;
using System.Windows;

namespace SimpleGuide;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        List<GuidVo> list = new List<GuidVo>()
        {
            new GuidVo()
            {
                Uc=btn1,
                Content="我是button，是第一步"
            },
            new GuidVo()
            {
                Uc=tb1,
                Content="我是textbox，是第二步"
            },
            new GuidVo()
            {
                Uc=rb1,
                Content="我是RadioButton，是第三步，也是最后一步了"
            }
        };

        GuideWin win = new GuideWin(this, list);

        win.ShowDialog();
    }
}