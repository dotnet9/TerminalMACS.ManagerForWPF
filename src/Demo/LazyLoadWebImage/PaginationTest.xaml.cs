using System.Windows;

namespace LazyLoadWebImage;

/// <summary>
///     Interaction logic for PaginationTest.xaml
/// </summary>
public partial class PaginationTest : Window
{
    public PaginationTest()
    {
        InitializeComponent();
        DataContext = new PaginationTestViewModel();
    }
}