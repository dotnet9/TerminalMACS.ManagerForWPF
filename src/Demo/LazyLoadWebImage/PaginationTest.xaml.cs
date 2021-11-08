using System.Windows;

namespace LazyLoadWebImage
{
	/// <summary>
	/// Interaction logic for PaginationTest.xaml
	/// </summary>
	public partial class PaginationTest : Window
	{
		public PaginationTest()
		{
			InitializeComponent();
			this.DataContext = new PaginationTestViewModel();
		}
	}
}
