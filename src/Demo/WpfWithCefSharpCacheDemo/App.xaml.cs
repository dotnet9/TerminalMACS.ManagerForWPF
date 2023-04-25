using CefSharp;
using CefSharp.Wpf;
using System;
using System.Windows;

namespace WpfWithCefSharpCacheDemo;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 默认缓存
        //var settings = new CefSettings
        //{
        //    CachePath = $"{AppDomain.CurrentDomain.BaseDirectory}DefaultCaches"
        //};
        //Cef.Initialize(settings);
    }
}