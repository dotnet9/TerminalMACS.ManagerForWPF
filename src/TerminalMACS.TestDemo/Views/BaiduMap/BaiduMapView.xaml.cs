using System;
using System.Windows;

namespace TerminalMACS.TestDemo.Views.BaiduMap
{
    /// <summary>
    /// BaiduMapView.xaml 的交互逻辑
    /// </summary>
    public partial class BaiduMapView : Window
    {
        public BaiduMapView()
        {
            InitializeComponent();

            OprateBasic OBasic = new OprateBasic(this);
            web.ObjectForScripting = OBasic;
            this.Loaded += (s, e) =>
            {
                var bmapHtmlFile = $"{AppDomain.CurrentDomain.BaseDirectory}BMap.html";
                if (System.IO.File.Exists(bmapHtmlFile))
                {
                    this.web.Navigate(bmapHtmlFile);
                }
                else
                {
                    MessageBox.Show($"Please copy BMap.html to output dir:{bmapHtmlFile }");
                }
            };
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            object[] objs = new object[2] {
                double.Parse(this.jin.Text),
                double.Parse(this.wei.Text)};
            web.InvokeScript("MoveToPoint", objs);

        }
        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            object[] objs = new object[2] {
                double.Parse(this.jin.Text),
                double.Parse(this.wei.Text)};
            web.InvokeScript("addMarker", objs);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            web.InvokeScript("removeOverlay", null);
        }
    }
}
