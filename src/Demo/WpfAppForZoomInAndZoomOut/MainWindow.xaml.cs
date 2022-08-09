using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfAppForZoomInAndZoomOut
{
    public partial class MainWindow : Window
    {
        List<TestModel> _itemSource = new List<TestModel>();

        public MainWindow()
        {
            InitializeComponent();
            var listCount = 100;
            for (var i = 0; i < listCount; i++)
            {
                _itemSource.Add(new TestModel { Index = i, Name = Helper.RandomString() });
            }

            listbox.ItemsSource = _itemSource;
            this.tbListInfo.Text = $"列表共有{listCount}条数据";
        }

        private void NewSelectedItemChanged(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var item = ((ListBoxItem)sender);
            var contenxt = (TestModel)item.DataContext;
            _itemSource.Where(x => !x.Equals(contenxt)).ToList().ForEach(x => x.IsSelected = false);
            contenxt.IsSelected = false;
        }


        private void Button80_Click(object sender, RoutedEventArgs e)
        {
            Scale(0.8);
        }

        private void Scale(double scale)
        {
            this.st.CenterX = this.st.CenterY = this.st.ScaleX = this.st.ScaleY = scale;
            this.listbox.Width = this.OuterScrollViewer.ActualWidth * scale;
            //this.listbox.Height = this.listbox.ActualHeight * scale;
            //var listBoxScrollViewer = this.listbox.FindChild<ScrollViewer>();
        }


        private void Button100_Click(object sender, RoutedEventArgs e)
        {
            Scale(1);
        }

        private void Button120_Click(object sender, RoutedEventArgs e)
        {
            Scale(1.2);
        }

        private void Button150_Click(object sender, RoutedEventArgs e)
        {
            Scale(1.5);
        }

        private void Button200_Click(object sender, RoutedEventArgs e)
        {
            Scale(2);
        }

        private void Button300_Click(object sender, RoutedEventArgs e)
        {
            Scale(3);
        }

        private void Window_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                return;
            }

            var slideValue = this.slider.Value;
            if (e.Delta > 0)
            {
                slideValue += 0.2;
            }
            else
            {
                slideValue -= 0.2;
            }

            if (slideValue < this.slider.Minimum)
            {
                slideValue = this.slider.Minimum;
            }

            if (slideValue > this.slider.Maximum)
            {
                slideValue = this.slider.Maximum;
            }

            Scale(slideValue);
        }

        private void listbox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                return;
            }

            var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArgs.RoutedEvent = UIElement.MouseWheelEvent;
            eventArgs.Source = sender;
            ((FrameworkElement)sender).RaiseEvent(eventArgs);
        }
    }
}