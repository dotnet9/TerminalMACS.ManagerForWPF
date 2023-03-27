using System;
using System.IO;
using CefSharp;
using System.Windows;

namespace WpfWithCefSharpDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.RegisterName("cefSharpExample", new CefSharpExample(this));
            var htmlFile = $"{AppDomain.CurrentDomain.BaseDirectory}test.html";
            if (File.Exists(htmlFile))
            {
                Browser.LoadHtml(htmlFile);
            }
        }
    }

    public class CefSharpExample
    {
        private MainWindow mainWindow;

        public CefSharpExample(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void TestMethod(string message)
        {
            mainWindow.Dispatcher.Invoke(() =>
            {
                var jsCode = $"displayMessage('{message}')";
                mainWindow.Browser.ExecuteScriptAsync(jsCode);
            });
        }
    }
}
