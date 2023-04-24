using System.Windows;
using WpfWithCefSharpCacheDemo.Caches;

namespace WpfWithCefSharpCacheDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestCefCacheView : Window
    {
        public TestCefCacheView()
        {
            InitializeComponent();

            var handler = new CefRequestHandlerc();
            CefBrowser.RequestHandler = handler;
        }

        private void LoadBaidu_Click(object sender, RoutedEventArgs e)
        {
            CefBrowser.Load("www.baidu.com");
        }

        private void LoadBaiduFanyi_Click(object sender, RoutedEventArgs e)
        {
            CefBrowser.Load("https://fanyi.baidu.com/");
        }

        private void LoadDotnet9Home_Click(object sender, RoutedEventArgs e)
        {
            CefBrowser.Load("https://dotnet9.com/");
        }

        private void LoadDotnet9About_Click(object sender, RoutedEventArgs e)
        {
            CefBrowser.Load("https://dotnet9.com/about");
        }
    }
}