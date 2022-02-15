using System.Windows;
using System.Windows.Controls;

namespace LazyLoadWebImage;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool isBusy;
    private int loadTimes;
    private readonly MainWindowViewModel viewModel = new();

    public MainWindow()
    {
        InitializeComponent();

        lvImages.ItemsSource = viewModel.ItemSource;
        viewModel.LoadMore();
        loadTimes++;
    }

    private void lvImages_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (!viewModel.NeedLoadMore || isBusy) return;

        isBusy = true;

        var atBottom = e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight - 50;
        if (atBottom && viewModel.NeedLoadMore)
        {
            loadTimes++;
            viewModel.LoadMore();
            Title = $"加载{loadTimes}次";
        }

        isBusy = false;
    }
}