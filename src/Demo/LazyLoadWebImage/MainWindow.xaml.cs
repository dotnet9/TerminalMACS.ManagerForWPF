using System;
using System.Windows;
using System.Windows.Controls;

namespace LazyLoadWebImage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainWindowViewModel viewModel = new MainWindowViewModel();
		private int loadTimes = 0;
		public MainWindow()
		{
			InitializeComponent();

			this.lvImages.ItemsSource = viewModel.ItemSource;
			this.viewModel.LoadMore();
			loadTimes++;
		}

		bool isBusy = false;
		private void lvImages_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
		{
			if (!this.viewModel.NeedLoadMore || isBusy)
			{
				return;
			}

			isBusy = true;

			bool atBottom = (e.VerticalOffset + e.ViewportHeight) >= e.ExtentHeight - 50;
			if (atBottom && this.viewModel.NeedLoadMore)
			{
				loadTimes++;
				this.viewModel.LoadMore();
				this.Title = $"加载{loadTimes}次";
			}
			isBusy = false;
		}
	}
}
