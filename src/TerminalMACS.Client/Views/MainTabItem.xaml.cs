using System.Windows;
using System.Windows.Documents;
using TerminalMACS.Client.Views.FlameDemo.Flame1;

namespace TerminalMACS.Client.Views;

public partial class MainTabItem
{
    private bool isLoad;

    public MainTabItem()
    {
        InitializeComponent();
    }

    // 参考链接：https://ask.csdn.net/questions/271708
    private void ShowFlame1_Click(object sender, RoutedEventArgs e)
    {
        if (!isLoad)
        {
            isLoad = true;

            AdornerLayer.GetAdornerLayer(spFlamePanel).Add(new FireAdorner(spFlamePanel));
        }
    }
}