using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleGuide;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowNormalWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuidWindow(20, 40)
        {
            Title = "有边框窗体测试引导"
        };
        window.Show();
    }

    private void ShowWithoutBorderWindowWithGuide_Click(object sender, RoutedEventArgs e)
    {
        var window = new WithGuidWindow
        {
            Title = "无边框窗体测试引导",
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None
        };
        window.Show();
    }
}