using System.Windows;
using System.Windows.Controls;

namespace SimpleGuide;

/// <summary>
/// Interaction logic for HintUC.xaml
/// </summary>
public partial class HintUC : UserControl
{
    public HintUC(string xh, string content, Visibility vis = Visibility.Visible, int width = 260, int height = 160)
    {
        InitializeComponent();
        this.Width = width;
        this.Height = height;
        this.tb_nr.Width = width / 4;

        this.tb_xh.Text = xh;
        this.tb_nr.Text = content;
        this.btn_next.Visibility = vis;
    }

    public delegate void NextHintDelegate();

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