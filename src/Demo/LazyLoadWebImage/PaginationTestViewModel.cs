using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace LazyLoadWebImage;

public class PaginationTestViewModel : BindableBase
{
    private int _pageCount = 101;
    private string _pageCountString = "共100项";
    private int _pageIndex = 1;
    private int _pageSize = 60;

    private readonly ObservableCollection<PageSizeInfo>? _pageSizeItemSource;
    private ICommand? _showCommand;

    /// <summary>
    ///     页码改变命令
    /// </summary>
    public ICommand ShowCommand => _showCommand ?? new DelegateCommand(RaiseShowCommand);

    public ObservableCollection<PageSizeInfo> PageSizeItemSource => _pageSizeItemSource ??
                                                                    new ObservableCollection<PageSizeInfo>
                                                                    {
                                                                        new(60, "60 Item / Page"),
                                                                        new(120, "120 Item / Page"),
                                                                        new(180, "180 Item / Page"),
                                                                        new(240, "240 Item / Page")
                                                                    };

    public int PageSize
    {
        get => _pageSize;
        set => SetProperty(ref _pageSize, value);
    }

    public string PageCountString
    {
        get => _pageCountString;
        set => SetProperty(ref _pageCountString, value);
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => SetProperty(ref _pageIndex, value);
    }

    public int PageCount
    {
        get => _pageCount;
        set => SetProperty(ref _pageCount, value);
    }


    /// <summary>
    ///     页码改变
    /// </summary>
    private void RaiseShowCommand()
    {
        MessageBox.Show($"{PageSizeItemSource.Count} + {PageSize}");
    }
}