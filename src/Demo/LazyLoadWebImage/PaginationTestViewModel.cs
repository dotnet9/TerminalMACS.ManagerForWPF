using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace LazyLoadWebImage;

public class PaginationTestViewModel : BindableBase
{
    private int _PageCount = 101;
    private string _PageCountString = "共100项";
    private int _PageIndex = 1;
    private int _pageSize = 60;

    private ObservableCollection<PageSizeInfo>? pageSizeItemSource;
    private ICommand showCommand;

    /// <summary>
    ///     页码改变命令
    /// </summary>
    public ICommand ShowCommand => showCommand ?? new DelegateCommand(async () => await RaiseShowCommand());

    public ObservableCollection<PageSizeInfo> PageSizeItemSource => pageSizeItemSource ??
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
        get => _PageCountString;
        set => SetProperty(ref _PageCountString, value);
    }

    public int PageIndex
    {
        get => _PageIndex;
        set => SetProperty(ref _PageIndex, value);
    }

    public int PageCount
    {
        get => _PageCount;
        set => SetProperty(ref _PageCount, value);
    }


    /// <summary>
    ///     页码改变
    /// </summary>
    private async Task RaiseShowCommand()
    {
        MessageBox.Show($"{PageSizeItemSource.Count} + {PageSize}");
    }
}