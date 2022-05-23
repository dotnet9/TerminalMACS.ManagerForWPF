using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace UserGuideForMVVM.Controls;

[TemplatePart(Name = PART_Bor, Type = typeof(Border))]
[TemplatePart(Name = PART_Can, Type = typeof(Canvas))]
public class GuideWindow : Window
{
    private const string PART_Bor = "PART_Bor";
    private const string PART_Can = "PART_Can";
    private readonly List<GuideInfo> list;

    private Border bor;

    private PathGeometry borGeometry = new();
    private Canvas canvas;
    private int index;

    static GuideWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideWindow),
            new FrameworkPropertyMetadata(typeof(GuideWindow)));
    }

    public GuideWindow(Window targetWindow, List<GuideInfo> guideList)
    {
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        ShowInTaskbar = false;

        //设置弹出的窗体
        Height = targetWindow.ActualHeight;
        Width = targetWindow.ActualWidth;
        WindowState = targetWindow.WindowState;
        Left = targetWindow.Left;
        Top = targetWindow.Top;
        Owner = targetWindow;
        list = guideList;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        bor = GetTemplateChild(PART_Bor) as Border;
        canvas = GetTemplateChild(PART_Can) as Canvas;

        var currentGuideInfo = list[index];
        ShowGuidArea(currentGuideInfo.Uc,  currentGuideInfo);
    }


    private void ShowGuidArea(FrameworkElement tagetControl, GuideInfo guide)
    {
        var point = tagetControl.TransformToAncestor(GetWindow(tagetControl)!).Transform(new Point(0, 0)); //获取控件坐标点

        var rg = new RectangleGeometry();
        rg.Rect = new Rect(0, 0, Width, Height);
        borGeometry = Geometry.Combine(borGeometry, rg, GeometryCombineMode.Union, null);
        bor.Clip = borGeometry;

        var rg1 = new RectangleGeometry();
        rg1.Rect = new Rect(point.X - 5, point.Y - 5, tagetControl.ActualWidth + 10, tagetControl.ActualHeight + 10);
        borGeometry = Geometry.Combine(borGeometry, rg1, GeometryCombineMode.Exclude, null);

        bor.Clip = borGeometry;

        var hit = new HintUc(this, point, tagetControl, guide);
        hit.NextHintEvent -= Hit_nextHintEvent;
        hit.NextHintEvent += Hit_nextHintEvent;
        canvas.Children.Add(hit);
    }

    private void Hit_nextHintEvent()
    {
        canvas.Children.Clear();
        if (index >= list.Count - 1)
        {
            this.Close();
            return;
        }

        index++;

        var currentGuideInfo = list[index];
        ShowGuidArea(currentGuideInfo.Uc, currentGuideInfo);
    }
}