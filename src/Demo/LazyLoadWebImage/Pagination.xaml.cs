using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LazyLoadWebImage;

public partial class Pagination : UserControl
{
    public static readonly DependencyProperty PageCountStringProperty = DependencyProperty.Register(
        "PageCountString", typeof(string), typeof(Pagination), new UIPropertyMetadata(""));

    public static readonly DependencyProperty PageSizeItemSourceProperty = DependencyProperty.Register(
        "PageSizeItemSource", typeof(List<PageSizeInfo>), typeof(Pagination),
        new UIPropertyMetadata(new List<PageSizeInfo>
            { new(60, "60 Item / Page"), new(120, "120 Item / Page"), new(80, "180 Item / Page") }));

    public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
        "PageSize", typeof(int), typeof(Pagination), new UIPropertyMetadata(60));

    public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
        "PageIndex", typeof(int), typeof(Pagination), new PropertyMetadata(1, OnPageIndexChanged, CoercePageIndex),
        ValidateHelper.IsInRangeOfPosIntIncludeZero);

    public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(
        "PageCount", typeof(int), typeof(Pagination), new PropertyMetadata(1, OnPageCountChanged, CoercePageCount),
        ValidateHelper.IsInRangeOfPosIntIncludeZero);

    public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(
        "MaxPageInterval", typeof(int), typeof(Pagination), new PropertyMetadata(3, OnMaxPageIntervalChanged),
        ValidateHelper.IsInRangeOfPosIntIncludeZero);

    private bool isBusy;

    public Pagination()
    {
        InitializeComponent();

        PartJump.SelectedIndex = 0;

        Update();
    }

    public int MaxPageInterval
    {
        get => (int)GetValue(MaxPageIntervalProperty);
        set => SetValue(MaxPageIntervalProperty, value);
    }

    #region PageCountString

    public string PageCountString
    {
        get => (string)GetValue(PageCountStringProperty);
        set => SetValue(PageCountStringProperty, value);
    }

    #endregion

    private static void OnMaxPageIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination) pagination.Update();
    }

    public override void OnApplyTemplate()
    {
        isBusy = true;
        base.OnApplyTemplate();

        isBusy = false;
        Update();
    }

    private void Update()
    {
        if (isBusy) return;

        PartButtonLast.Content = PartButtonLast.Tag = PageCount;
        PartButtonLastSecond.Content = PartButtonLastSecond.Tag = PageCount - 1;
        PartButtonLeft.IsEnabled = PageIndex > 1;
        PartButtonRight.IsEnabled = PageIndex < PageCount;
        if (MaxPageInterval == 0)
        {
            PartButtonFirst.Collapse();
            PartButtonSecond.Collapse();
            PartButtonLastSecond.Collapse();
            PartButtonLast.Collapse();
            PartMoreLeft.Collapse();
            PartMoreRight.Collapse();
            PartPanelMain.Children.Clear();
            var selectButton = CreateButton(PageIndex);
            PartPanelMain.Children.Add(selectButton);
            selectButton.IsChecked = true;
            return;
        }

        PartButtonFirst.Show();
        PartButtonSecond.Show(PageCount > 1);
        PartButtonLastSecond.Show();
        PartButtonLast.Show();
        PartMoreLeft.Show();
        PartMoreRight.Show();

        if (PageCount is 1 or 2)
        {
            PartButtonLastSecond.Collapse();
            PartButtonLast.Collapse();
        }
        else
        {
            PartButtonLastSecond.Show();
            PartButtonLast.Show();
            PartButtonLastSecond.Content = PartButtonLastSecond.Tag = (PageCount - 1).ToString();
            PartButtonLast.Content = PartButtonLast.Tag = PageCount.ToString();
        }

        var right = PageCount - PageIndex - 2;
        var left = PageIndex - 3;
        var isShowMoreRight = right > MaxPageInterval;
        PartMoreRight.Show(isShowMoreRight);
        if (isShowMoreRight) PartMoreRight.Tag = PageIndex + MaxPageInterval + 1;
        var isShowMoreLeft = left > MaxPageInterval;
        PartMoreLeft.Show(isShowMoreLeft);
        if (isShowMoreLeft) PartMoreLeft.Tag = PageIndex - MaxPageInterval - 1;

        PartPanelMain.Children.Clear();
        if (PageIndex == 1)
        {
            PartButtonFirst.IsChecked = true;
        }
        else if (PageIndex == 2)
        {
            PartButtonSecond.IsChecked = true;
        }
        else if (PageIndex > 2 && PageIndex < PageCount - 1)
        {
            var selectButton = CreateButton(PageIndex);
            PartPanelMain.Children.Add(selectButton);
            selectButton.IsChecked = true;
        }
        else if (PageIndex == PageCount - 1)
        {
            PartButtonLastSecond.IsChecked = true;
        }
        else
        {
            PartButtonLast.IsChecked = true;
        }

        var sub = PageIndex;
        for (var i = 0; i < MaxPageInterval; i++)
        {
            --sub;
            if (sub > 2 && sub < PageCount - 1) PartPanelMain.Children.Insert(0, CreateButton(sub));
        }

        var add = PageIndex;
        for (var i = 0; i < MaxPageInterval; i++)
        {
            ++add;
            if (add > 2 && add < PageCount - 1) PartPanelMain.Children.Add(CreateButton(add));
        }
    }

    private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
    {
        PageIndex--;
    }

    private void ButtonPrev_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            ButtonPrev_OnClick(null, null);
            e.Handled = true;
        }
    }

    private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
    {
        PageIndex++;
    }

    private void ButtonNext_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Right)
        {
            ButtonNext_OnClick(null, null);
            e.Handled = true;
        }
    }

    private RadioButton CreateButton(int page)
    {
        var rb = new RadioButton { Content = page.ToString(), Tag = page };
        rb.Checked += ToggleButton_OnChecked;
        return rb;
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not RadioButton button) return;

        if (button.IsChecked == false) return;

        PageIndex = int.Parse(button.Tag.ToString());
    }

    private void PartJumpPage_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        if (!int.TryParse(PartJumpPage.Text, out var page)) return;
        e.Handled = true;
        if (page < 1)
            PageIndex = 1;
        else if (page > PageCount)
            PageIndex = PageCount;
        else if (page != PageIndex) PageIndex = page;
    }

    #region PageSize

    public List<PageSizeInfo> PageSizeItemSource
    {
        get => (List<PageSizeInfo>)GetValue(PageSizeItemSourceProperty);
        set => SetValue(PageSizeItemSourceProperty, value);
    }

    public int PageSize
    {
        get => (int)GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    #endregion

    #region PageIndex

    private static object CoercePageIndex(DependencyObject d, object basevalue)
    {
        if (d is not Pagination pagination) return 1;

        var intValue = (int)basevalue;
        if (intValue < 1) return 1;
        if (intValue > pagination.PageCount) return pagination.PageCount;
        return intValue;
    }

    private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination && e.NewValue is int value) pagination.Update();
    }

    public int PageIndex
    {
        get => (int)GetValue(PageIndexProperty);
        // 使用SetCurrentValue原因参考：https://wpf.2000things.com/2010/12/06/147-use-setcurrentvalue-when-you-want-to-set-a-dependency-property-value-from-within-a-control/
        set => SetCurrentValue(PageIndexProperty, value);
    }

    #endregion

    #region PageCount

    private static object CoercePageCount(DependencyObject d, object basevalue)
    {
        var intValue = (int)basevalue;
        return intValue < 1 ? 1 : intValue;
    }

    private static void OnPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination pagination)
        {
            if (pagination.PageIndex > pagination.PageCount) pagination.PageIndex = pagination.PageCount;

            pagination.CoerceValue(PageIndexProperty);
            pagination.Update();
        }
    }

    public int PageCount
    {
        get => (int)GetValue(PageCountProperty);
        set => SetValue(PageCountProperty, value);
    }

    #endregion
}

// ReSharper disable once InconsistentNaming
public static class UIElementExtension
{
    public static void Show(this UIElement element)
    {
        element.Visibility = Visibility.Visible;
    }

    public static void Show(this UIElement element, bool show)
    {
        element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
    }

    public static void Hide(this UIElement element)
    {
        element.Visibility = Visibility.Hidden;
    }

    public static void Collapse(this UIElement element)
    {
        element.Visibility = Visibility.Collapsed;
    }
}

public class ValidateHelper
{
    public static bool IsInRangeOfPosIntIncludeZero(object value)
    {
        var v = (int)value;
        return v >= 0;
    }
}

public class PageSizeInfo
{
    public PageSizeInfo(int pageSize, string text)
    {
        PageSize = pageSize;
        Text = text;
    }

    public string? Text { get; set; }

    public int PageSize { get; set; }
}