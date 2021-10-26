using System.Windows;

namespace LazyLoadWebImage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private bool isLoaded = false;
		private void SetBinding_Click(object sender, RoutedEventArgs e)
		{
			if(isLoaded)
			{
				return;
			}
			isLoaded = true;

			MainWindowViewModel viewModel = new MainWindowViewModel();
			this.lvImages.ItemsSource = viewModel.WebImages;
		}
	}
}
