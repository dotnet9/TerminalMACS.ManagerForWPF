using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UserGuideForMVVM.Controls;

[TemplatePart(Name = PART_Viewbox, Type = typeof(Viewbox))]
[TemplatePart(Name = PART_Padgrid, Type = typeof(Grid))]
[TemplatePart(Name = PART_Btn_stack, Type = typeof(StackPanel))]
[TemplatePart(Name = PART_Btn_next, Type = typeof(Button))]
public class HintUc : Control
{
    public delegate void NextHintDelegate();

    private const string PART_Viewbox = "PART_Viewbox";
    private const string PART_Padgrid = "PART_Padgrid";
    private const string PART_Btn_stack = "PART_Btn_stack";
    private const string PART_Btn_next = "PART_Btn_next";

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(HintUc), new PropertyMetadata(null));

    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(string), typeof(HintUc), new PropertyMetadata(null));

    public static readonly DependencyProperty NextContentProperty =
        DependencyProperty.Register(nameof(NextContent), typeof(string), typeof(HintUc), new PropertyMetadata("下一步"));

    private readonly FrameworkElement _fe;

    private readonly Window _ownerWindow;
    private Point _point;
    private Button btnNext;
    private Grid padGrid;

    private Viewbox viewbox;

    static HintUc()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HintUc), new FrameworkPropertyMetadata(typeof(HintUc)));
    }

    public HintUc(Window ownerWindow, Point point, FrameworkElement targetControl, GuideInfo guide)
    {
        _ownerWindow = ownerWindow;
        _point = point;
        _fe = targetControl;

        if (guide.Width != null) Width = guide.Width.Value;
        if (guide.Height != null) Height = guide.Height.Value;

        Title = guide.Title;
        Content = guide.Content;
        NextContent = guide.ButtonContent;

        Loaded += HintUC_Loaded;
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Content
    {
        get => (string)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    ///     下一步按钮内容
    /// </summary>
    public string NextContent
    {
        get => (string)GetValue(NextContentProperty);
        set => SetValue(NextContentProperty, value);
    }

    private void HintUC_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= HintUC_Loaded;
        var left1 = _point.X + _fe.ActualWidth + 5;
        var left2 = _point.X - ActualWidth - 5;
        var top1 = _point.Y + _fe.ActualHeight - 12;
        var top2 = _point.Y - ActualHeight + 12;

        if (ActualWidth + left1 <= _ownerWindow.Width && ActualHeight + top1 <= _ownerWindow.Height)
        {
            Canvas.SetLeft(this, left1);
            Canvas.SetTop(this, top1);
        }
        else if (ActualWidth + left1 <= _ownerWindow.Width && top2 >= 0)
        {
            Canvas.SetLeft(this, left1);
            Canvas.SetTop(this, top2);

            var scaleTransform = new ScaleTransform();
            scaleTransform.ScaleY = -1;
            viewbox.RenderTransform = scaleTransform;
        }
        else if (left2 >= 0 && ActualHeight + top1 <= _ownerWindow.Height)
        {
            Canvas.SetLeft(this, left2);
            Canvas.SetTop(this, top1);

            var scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = -1;
            viewbox.RenderTransform = scaleTransform;

            DockPanel.SetDock(padGrid, Dock.Right);
        }
        else if (left2 >= 0 && top2 >= 0)
        {
            Canvas.SetLeft(this, left2);
            Canvas.SetTop(this, top2);

            var scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = -1;
            scaleTransform.ScaleY = -1;
            viewbox.RenderTransform = scaleTransform;

            DockPanel.SetDock(padGrid, Dock.Right);
        }
        else //怎么放都不行，就按第一种放吧
        {
            Canvas.SetLeft(this, left1);
            Canvas.SetTop(this, top1);
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (btnNext != null) btnNext.Click -= btn_next_Click;

        viewbox = GetTemplateChild(PART_Viewbox) as Viewbox;
        padGrid = GetTemplateChild(PART_Padgrid) as Grid;
        btnNext = GetTemplateChild(PART_Btn_next) as Button;

        if (btnNext != null) btnNext.Click += btn_next_Click;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        btnNext.Focus();
    }

    public event NextHintDelegate? NextHintEvent;

    private void btn_next_Click(object sender, RoutedEventArgs e)
    {
        NextHintEvent?.Invoke();
    }
}