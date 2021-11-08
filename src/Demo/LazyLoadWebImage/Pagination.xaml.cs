namespace LazyLoadWebImage
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class Pagination : UserControl
    {
        public static readonly DependencyProperty PageCountStringProperty = DependencyProperty.Register(
            "PageCountString", typeof(string), typeof(Pagination), new UIPropertyMetadata(""));

        public static readonly DependencyProperty PageSizeItemSourceProperty = DependencyProperty.Register(
            "PageSizeItemSource", typeof(List<PageSizeInfo>), typeof(Pagination),
            new UIPropertyMetadata(new List<PageSizeInfo> { new PageSizeInfo(60, "60 Item / Page"), new PageSizeInfo(120, "120 Item / Page"), new PageSizeInfo(80, "180 Item / Page") }));

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
            "PageSize", typeof(int), typeof(Pagination), new UIPropertyMetadata(60));

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(Pagination), new PropertyMetadata(1, OnPageIndexChanged, CoercePageIndex), ValidateHelper.IsInRangeOfPosIntIncludeZero);

        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(
            "PageCount", typeof(int), typeof(Pagination), new PropertyMetadata(1, OnPageCountChanged, CoercePageCount), ValidateHelper.IsInRangeOfPosIntIncludeZero);

        public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(
            "MaxPageInterval", typeof(int), typeof(Pagination), new PropertyMetadata(3, OnMaxPageIntervalChanged), ValidateHelper.IsInRangeOfPosIntIncludeZero);

        private bool isBusy;

        public Pagination()
        {
            this.InitializeComponent();

            this.PartJump.SelectedIndex = 0;

            this.Update();
        }

        public int MaxPageInterval
        {
            get => (int)this.GetValue(MaxPageIntervalProperty);
            set => this.SetValue(MaxPageIntervalProperty, value);
        }

        private static void OnMaxPageIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                pagination.Update();
            }
        }

        public override void OnApplyTemplate()
        {
            this.isBusy = true;
            base.OnApplyTemplate();

            this.isBusy = false;
            this.Update();
        }

        private void Update()
        {
            if (this.isBusy)
            {
                return;
            }

            this.PartButtonLast.Content = this.PartButtonLast.Tag = this.PageCount;
            this.PartButtonLastSecond.Content = this.PartButtonLastSecond.Tag = this.PageCount - 1;
            this.PartButtonLeft.IsEnabled = this.PageIndex > 1;
            this.PartButtonRight.IsEnabled = this.PageIndex < this.PageCount;
            if (this.MaxPageInterval == 0)
            {
                this.PartButtonFirst.Collapse();
                this.PartButtonSecond.Collapse();
                this.PartButtonLastSecond.Collapse();
                this.PartButtonLast.Collapse();
                this.PartMoreLeft.Collapse();
                this.PartMoreRight.Collapse();
                this.PartPanelMain.Children.Clear();
                var selectButton = this.CreateButton(this.PageIndex);
                this.PartPanelMain.Children.Add(selectButton);
                selectButton.IsChecked = true;
                return;
            }
            this.PartButtonFirst.Show();
            this.PartButtonSecond.Show(this.PageCount > 1);
            this.PartButtonLastSecond.Show();
            this.PartButtonLast.Show();
            this.PartMoreLeft.Show();
            this.PartMoreRight.Show();

            if (this.PageCount is 1 or 2)
            {
                this.PartButtonLastSecond.Collapse();
                this.PartButtonLast.Collapse();
            }
            else
            {
                this.PartButtonLastSecond.Show();
                this.PartButtonLast.Show();
                this.PartButtonLastSecond.Content = this.PartButtonLastSecond.Tag = (this.PageCount - 1).ToString();
                this.PartButtonLast.Content = this.PartButtonLast.Tag = this.PageCount.ToString();
            }

            var right = this.PageCount - this.PageIndex - 2;
            var left = this.PageIndex - 3;
            var isShowMoreRight = right > this.MaxPageInterval;
            this.PartMoreRight.Show(isShowMoreRight);
            if (isShowMoreRight)
            {
                this.PartMoreRight.Tag = this.PageIndex + this.MaxPageInterval + 1;
            }
            var isShowMoreLeft = left > this.MaxPageInterval;
            this.PartMoreLeft.Show(isShowMoreLeft);
            if (isShowMoreLeft)
            {
                this.PartMoreLeft.Tag = this.PageIndex - this.MaxPageInterval - 1;
            }

            this.PartPanelMain.Children.Clear();
            if (this.PageIndex == 1)
            {
                this.PartButtonFirst.IsChecked = true;
            }
            else if (this.PageIndex == 2)
            {
                this.PartButtonSecond.IsChecked = true;
            }
            else if (this.PageIndex > 2 && this.PageIndex < this.PageCount - 1)
            {
                var selectButton = this.CreateButton(this.PageIndex);
                this.PartPanelMain.Children.Add(selectButton);
                selectButton.IsChecked = true;
            }
            else if (this.PageIndex == this.PageCount - 1)
            {
                this.PartButtonLastSecond.IsChecked = true;
            }
            else
            {
                this.PartButtonLast.IsChecked = true;
            }

            var sub = this.PageIndex;
            for (var i = 0; i < this.MaxPageInterval; i++)
            {
                --sub;
                if (sub > 2 && sub < this.PageCount - 1)
                {
                    this.PartPanelMain.Children.Insert(0, this.CreateButton(sub));
                }
            }

            var add = this.PageIndex;
            for (var i = 0; i < this.MaxPageInterval; i++)
            {
                ++add;
                if (add > 2 && add < this.PageCount - 1)
                {
                    this.PartPanelMain.Children.Add(this.CreateButton(add));
                }
            }
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
        {
            this.PageIndex--;
        }

        private void ButtonPrev_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                this.ButtonPrev_OnClick(null, null);
                e.Handled = true;
            }
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            this.PageIndex++;
        }

        private void ButtonNext_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                this.ButtonNext_OnClick(null, null);
                e.Handled = true;
            }
        }

        private RadioButton CreateButton(int page)
        {
            var rb = new RadioButton { Content = page.ToString(), Tag = page };
            rb.Checked += this.ToggleButton_OnChecked;
            return rb;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not RadioButton button)
            {
                return;
            }

            if (button.IsChecked == false)
            {
                return;
            }

            this.PageIndex = int.Parse(button.Tag.ToString());
        }

        private void PartJumpPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (!int.TryParse(this.PartJumpPage.Text, out var page))
            {
                return;
            }
            e.Handled = true;
            if (page < 1)
            {
                this.PageIndex = 1;
            }
            else if (page > this.PageCount)
            {
                this.PageIndex = this.PageCount;
            }
            else if (page != this.PageIndex)
            {
                this.PageIndex = page;
            }
        }

        #region PageCountString

        public string PageCountString
        {
            get => (string)this.GetValue(PageCountStringProperty);
            set => this.SetValue(PageCountStringProperty, value);
        }

        #endregion

        #region PageSize

        public List<PageSizeInfo> PageSizeItemSource
        {
            get => (List<PageSizeInfo>)this.GetValue(PageSizeItemSourceProperty);
            set => this.SetValue(PageSizeItemSourceProperty, value);
        }

        public int PageSize
        {
            get => (int)this.GetValue(PageSizeProperty);
            set => this.SetValue(PageSizeProperty, value);
        }

        #endregion

        #region PageIndex

        private static object CoercePageIndex(DependencyObject d, object basevalue)
        {
            if (d is not Pagination pagination)
            {
                return 1;
            }

            var intValue = (int)basevalue;
            if (intValue < 1)
            {
                return 1;
            }
            if (intValue > pagination.PageCount)
            {
                return pagination.PageCount;
            }
            return intValue;
        }

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination && e.NewValue is int value)
            {
                pagination.Update();
            }
        }

        public int PageIndex
        {
            get => (int)this.GetValue(PageIndexProperty);
            set => this.SetValue(PageIndexProperty, value);
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
                if (pagination.PageIndex > pagination.PageCount)
                {
                    pagination.PageIndex = pagination.PageCount;
                }

                pagination.CoerceValue(PageIndexProperty);
                pagination.Update();
            }
        }

        public int PageCount
        {
            get => (int)this.GetValue(PageCountProperty);
            set => this.SetValue(PageCountProperty, value);
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
            this.PageSize = pageSize;
            this.Text = text;
        }

        public string? Text { get; set; }

        public int PageSize { get; set; }
    }
}