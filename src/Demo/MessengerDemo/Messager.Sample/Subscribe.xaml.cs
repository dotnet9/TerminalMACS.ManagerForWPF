using System;
using System.Windows;

namespace Messager.Sample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class Subscribe : Window
	{
		public Subscribe()
		{
			InitializeComponent();
		}

		private string tag
		{
			get
			{
				return string.IsNullOrWhiteSpace(this.tbTag.Text.Trim()) ? null : this.tbTag.Text.Trim();
			}
		}
		private void Subscribe_Click(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Subscribe<TestMessage>(this, (msg) =>
		   {
			   this.tbSubscribe.AppendText(msg.Msg + "\r\n");
		   }, ThreadOption.PublisherThread, tag);
		}

		private void SubscribeInThread_Click(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Subscribe<TestMessage>(this, (msg) =>
			{
				this.tbSubscribe.AppendText(msg.Msg + "\r\n");
			}, ThreadOption.UiThread, tag);
		}

		private void Unsubscribe_Click(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Unsubscribe<TestMessage>(this, null);
		}


		private void OpenPublishWindow_Click(object sender, RoutedEventArgs e)
		{
			new Publish().Show();
		}

		private void OpenSubscribeWindow_Click(object sender, RoutedEventArgs e)
		{
			new Subscribe().Show();
		}
	}
}
