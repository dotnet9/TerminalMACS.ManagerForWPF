using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleGuide;

/// <summary>
/// Interaction logic for GuideWin.xaml
/// </summary>
public partial class GuideWin : Window
{
    PathGeometry borGeometry = new PathGeometry();
    List<GuidVo> list;
    int index = 0;
    int offsetLeft = 0;
    int offsetTop = 0;

    public GuideWin(Window win, List<GuidVo> gl, int offsetLeft = 0, int offsetTop = 0)
    {
        InitializeComponent();
        //设置弹出的窗体
        this.Height = win.ActualHeight;
        this.Width = win.ActualWidth;
        this.WindowState = win.WindowState;
        this.Left = win.Left;
        this.Top = win.Top;
        this.Owner = win;
        list = gl;
        ShowGuidArea(index + 1, list[index].Uc, list[index].Content);
        this.offsetLeft = offsetLeft;
        this.offsetTop = offsetTop;
    }

    private void ShowGuidArea(int xh, FrameworkElement fe, string con, Visibility vis = Visibility.Visible)
    {
        Point point = fe.TransformToAncestor(Window.GetWindow(fe)).Transform(new Point(0, 0)); //获取控件坐标点

        RectangleGeometry rg = new RectangleGeometry();
        rg.Rect = new Rect(0, 0, this.Width, this.Height);
        borGeometry = Geometry.Combine(borGeometry, rg, GeometryCombineMode.Union, null);
        bor.Clip = borGeometry;

        RectangleGeometry rg1 = new RectangleGeometry();
        rg1.Rect = new Rect(point.X - 5 + offsetLeft, point.Y - 5 + offsetTop, fe.ActualWidth + 10,
            fe.ActualHeight + 10);
        borGeometry = Geometry.Combine(borGeometry, rg1, GeometryCombineMode.Exclude, null);

        bor.Clip = borGeometry;

        HintUC hit = new HintUC(xh.ToString(), con, vis);
        Canvas.SetLeft(hit, point.X + fe.ActualWidth + 3);
        Canvas.SetTop(hit, point.Y + fe.ActualHeight + 3);
        hit.nextHintEvent -= Hit_nextHintEvent;
        hit.nextHintEvent += Hit_nextHintEvent;
        can.Children.Add(hit);

        index++;
    }

    private void Hit_nextHintEvent()
    {
        if (list[index - 1] != null)
        {
            can.Children.Clear();
        }

        if (index == list.Count - 1)
            ShowGuidArea(index + 1, list[index].Uc, list[index].Content, Visibility.Collapsed);
        else
            ShowGuidArea(index + 1, list[index].Uc, list[index].Content);
    }
}