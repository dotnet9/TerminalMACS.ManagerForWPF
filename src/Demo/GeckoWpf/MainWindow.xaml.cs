using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gecko;

namespace GeckoWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Gecko.Xpcom.Initialize("Firefox");
		}

		private GeckoWebBrowser browser = null;
		private void GridWeb_OnLoaded(object sender, RoutedEventArgs e)
		{
			WindowsFormsHost host = new WindowsFormsHost()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top
			};
			browser = new GeckoWebBrowser();
			host.Child = browser;
			GridWeb.Children.Add(host);
			browser.Load += (s, ee) =>
			{

				host.HorizontalAlignment = HorizontalAlignment.Stretch;
				host.VerticalAlignment = VerticalAlignment.Stretch;
			};
			browser.Navigate("www.baidu.com");
		}

		private void BtnChangeToBaidu_OnClick(object sender, RoutedEventArgs e)
		{
			browser.Navigate("www.baidu.com");
		}
		private void BtnChangeToMi_OnClick(object sender, RoutedEventArgs e)
		{
			browser.Navigate("https://www.mi.com/");
		}
	}
}
