using System.Windows;
using System.Windows.Controls;

namespace SimpleGuide;

public partial class HintUC : UserControl
{
    public delegate void NextHintDelegate();

    public HintUC(string xh, string content, Visibility vis = Visibility.Visible, int width = 260, int height = 160)
    {
        InitializeComponent();
        Width = width;
        Height = height;
        tb_nr.Width = width / 4;

        tb_xh.Text = xh;
        tb_nr.Text = content;
        btn_next.Visibility = vis;
    }

    public event NextHintDelegate nextHintEvent;

    private void btn_next_Click(object sender, RoutedEventArgs e)
    {
        nextHintEvent();
    }

    private void btn_close_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }
}