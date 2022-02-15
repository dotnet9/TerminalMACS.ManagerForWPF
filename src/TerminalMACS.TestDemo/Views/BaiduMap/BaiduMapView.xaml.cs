using System;
using System.IO;
using System.Windows;

namespace TerminalMACS.TestDemo.Views.BaiduMap;

/// <summary>
///     BaiduMapView.xaml 的交互逻辑
/// </summary>
public partial class BaiduMapView : Window
{
    public BaiduMapView()
    {
        InitializeComponent();

        var OBasic = new OprateBasic(this);
        web.ObjectForScripting = OBasic;
        Loaded += (s, e) =>
        {
            var bmapHtmlFile = $"{AppDomain.CurrentDomain.BaseDirectory}BMap.html";
            if (File.Exists(bmapHtmlFile))
                web.Navigate(bmapHtmlFile);
            else
                MessageBox.Show($"Please copy BMap.html to output dir:{bmapHtmlFile}");
        };
    }

    private void Move_Click(object sender, RoutedEventArgs e)
    {
        var objs = new object[2]
        {
            double.Parse(jin.Text),
            double.Parse(wei.Text)
        };
        web.InvokeScript("MoveToPoint", objs);
    }

    private void Mark_Click(object sender, RoutedEventArgs e)
    {
        var objs = new object[2]
        {
            double.Parse(jin.Text),
            double.Parse(wei.Text)
        };
        web.InvokeScript("addMarker", objs);
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        web.InvokeScript("removeOverlay", null);
    }
}