using Serilog;
using System.Diagnostics;
using System.Windows;

namespace WPFWithLogDashboard
{
	public partial class MainWindow : Window
	{
		private ILogger logger;
		public ILogger MyLoger
		{
			get
			{
				if (logger == null)
				{
					logger = Log.ForContext<MainWindow>();
				}
				return logger;
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			MyLoger.Information("WPF窗体中记录的日志");
		}
		private void AddInfoLog_Click(object sender, RoutedEventArgs e)
		{
			MyLoger.Information("测试添加Info日志");

		}

		private void AddErrorLog_Click(object sender, RoutedEventArgs e)
		{
			MyLoger.Error("测试添加异常日志");
		}

		private void OpenLogDashboard_Click(object sender, RoutedEventArgs e)
		{
			OpenUrl("http://localhost:5000/logdashboard");
		}

		private void OpenUrl(string url)
		{
			Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
			{
				UseShellExecute = false,
				CreateNoWindow = true
			});
		}
	}
}
