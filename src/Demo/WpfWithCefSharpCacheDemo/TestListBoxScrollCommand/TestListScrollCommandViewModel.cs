using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfWithCefSharpCacheDemo.TestListBoxScrollCommand;

public class TestListScrollCommandViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private ObservableCollection<string> _myItems;

    public ObservableCollection<string> MyItems
    {
        get { return _myItems; }
        set
        {
            _myItems = value;
            OnPropertyChanged(nameof(MyItems));
        }
    }

    private string _selectedItem;

    public string SelectedItem
    {
        get { return _selectedItem; }
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    private ICommand? _scrollCommand;

    public ICommand ScrollCommand
    {
        get { return _scrollCommand ??= new RelayCommand<ScrollChangedEventArgs>(Scroll); }
    }

    public TestListScrollCommandViewModel()
    {
        MyItems = new ObservableCollection<string>();
        for (int i = 0; i < 100; i++)
        {
            MyItems.Add($"Item {i}");
        }
    }

    bool IsScrollToBottom(ScrollChangedEventArgs e)
    {
        var verticalOffset = e.VerticalOffset;
        var viewportHeight = e.ViewportHeight;
        var extentHeight = e.ExtentHeight;
        return verticalOffset + viewportHeight >= extentHeight;
    }

    private void Scroll(ScrollChangedEventArgs e)
    {
        if (IsScrollToBottom(e))
        {
            MessageBox.Show("滚动到底，这里可以请求分页数据了");
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}