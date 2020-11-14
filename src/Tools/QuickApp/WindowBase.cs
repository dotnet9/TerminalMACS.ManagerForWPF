using System.Windows;
using System.Windows.Media;

namespace QuickApp
{
    public class WindowBase : Window
    {
        public WindowBase()
        {
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.FontSize = 14;
            this.Foreground = Brushes.White;
            this.ShowInTaskbar = false;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.WindowState = System.Windows.WindowState.Maximized;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //this.Topmost = true;
            //Activate();
            this.FontFamily = new FontFamily("Microsoft Yahei");
        }

    }
}
