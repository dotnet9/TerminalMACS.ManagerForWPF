using CefSharp;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace WpfWithCefSharpDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 允许以同步的方式注册C#的对象到JS中
            Browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;

            // 注册C#的对象到JS中的代码必须在Cef的Browser加载之前调用
            Browser.JavascriptObjectRepository.Register("cefSharpExample", new CefSharpExample(), false,
                options: BindingOptions.DefaultBinder);
        }

        /// <summary>
        /// Cef浏览器控件加载完成后，加载网页内容，可以加载网页的Url，也可以加载网页内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_OnLoaded(object sender, RoutedEventArgs e)
        {
            var htmlFile = $"{AppDomain.CurrentDomain.BaseDirectory}test.html";
            if (!File.Exists(htmlFile))
            {
                return;
            }

            var htmlContent = File.ReadAllText(htmlFile, Encoding.UTF8);
            Browser.LoadHtml(htmlContent);
        }

        /// <summary>
        /// C#里调用JS的一般方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallJSFunc_Click(object sender, RoutedEventArgs e)
        {
            var jsCode = $"displayMessage('C#里的调用')";
            Browser.ExecuteScriptAsync(jsCode);
        }

        /// <summary>
        /// C#调用一个JS的方法，并传递一个JSON对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendJsonToWeb_Click(object sender, RoutedEventArgs e)
        {
            var jsonContent = new
            {
                Id = 1,
                Name = "沙漠尽头的狼",
                Age = 25,
                WebSite="https://dotnet9.com"
            };
            var jsonStr = JsonConvert.SerializeObject(jsonContent);

            // 传递Json对象，即传递一个JSON字符串，和前面的一个示例一样
            var jsCode = $"displayJson('{jsonStr}')";
            Browser.ExecuteScriptAsync(jsCode);
        }
    }

    public class CefSharpExample
    {
        public void TestMethod(string message)
        {
            Application.Current.Dispatcher.Invoke(() => { MessageBox.Show("JS里的调用"); });
        }
    }
}