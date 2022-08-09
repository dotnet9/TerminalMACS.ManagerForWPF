using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppForZoomInAndZoomOut
{
    public partial class MainWindow : Window
    {
        List<Test> lst = new List<Test>();

        public MainWindow()
        {
            InitializeComponent();
            for (var i = 0; i < 100; i++)
            {
                lst.Add(new Test { Index = i, Name = RandomString(rd.Next(10, 50)) });
            }

            listbox.ItemsSource = lst;
        }

        private void NewSelectedItemChanged(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var item = ((ListBoxItem)sender);
            var contenxt = (Test)item.DataContext;
            lst.Where(x => !x.Equals(contenxt)).ToList().ForEach(x => x.IsSelected = false);
            contenxt.IsSelected = false;
        }

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random rd = new Random(DateTime.Now.Millisecond);

        public string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rd.Next(s.Length)]).ToArray());
        }

        private void Button80_Click(object sender, RoutedEventArgs e)
        {
            Scale(0.8);
        }

        private void Scale(double scale)
        {
            this.st.CenterX = this.st.CenterY = this.st.ScaleX = this.st.ScaleY = scale;
            var listBoxScrollViewer = FindChild<ScrollViewer>(this.listbox);
            Console.WriteLine($"\r\nScale: {scale}");
            Console.WriteLine($"listbox width x height: {listbox.ActualWidth} vs {listbox.ActualHeight}");
            Console.WriteLine("property: listBoxScrollViewer   vs    OuterScrollViewer ");
            Console.WriteLine($"ExtentHeight: {listBoxScrollViewer.ExtentHeight} vs {OuterScrollViewer.ExtentHeight}");
            Console.WriteLine($"ExtentWidth: {listBoxScrollViewer.ExtentWidth} vs {OuterScrollViewer.ExtentWidth}");
            Console.WriteLine($"ScrollableHeight: {listBoxScrollViewer.ScrollableHeight} vs {OuterScrollViewer.ScrollableHeight}");
            Console.WriteLine($"ScrollableWidth: {listBoxScrollViewer.ScrollableWidth} vs {OuterScrollViewer.ScrollableWidth}");
            Console.WriteLine($"width: {listBoxScrollViewer.ActualWidth} vs {OuterScrollViewer.ActualWidth}");
            Console.WriteLine($"height: {listBoxScrollViewer.ActualHeight} vs {OuterScrollViewer.ActualHeight}");
        }

        public T FindChild<T>(DependencyObject parent, string childName = null)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);
                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (child is IFrameworkInputElement frameworkInputElement &&
                        frameworkInputElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        // recursively drill down the tree
                        foundChild = FindChild<T>(child, childName);
                        // If the child is found, break so we do not overwrite the found child. 
                        if (foundChild != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
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
