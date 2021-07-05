using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messager.Sample
{
	/// <summary>
	/// Interaction logic for Publish.xaml
	/// </summary>
	public partial class Publish : Window
	{
		public Publish()
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
		private void Publish_Click(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Publish(this, new TestMessage(this, this.tbPublish.Text), tag);
		}

		private void OpenPublishWindow_Click(object sender, RoutedEventArgs e)
		{
			new Publish().Show();
		}

		private void OpenSubscribeWindow_Click(object sender, RoutedEventArgs e)
		{
			new Subscribe().Show();
		}

		private void PublishInThread_Click(object sender, RoutedEventArgs e)
		{
			var msg = this.tbPublish.Text;
			var tmpTag = tag;
			Task.Run(() =>
			{
				Messenger.Default.Publish(this, new TestMessage(this, msg), tmpTag);
			});

		}
	}
}
