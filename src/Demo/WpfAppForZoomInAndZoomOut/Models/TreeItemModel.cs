using System.Windows;
using Prism.Mvvm;

namespace WpfAppForZoomInAndZoomOut.Models;

public class TreeItemModel : BindableBase
{
    private int _caretIndex;
    private int _index;

    private bool _isSelected;

    private Thickness _margin;

    private string? _name;

    private int _selectionLength;

    public int Index
    {
        get => _index;
        set => SetProperty(ref _index, value);
    }

    public Thickness Margin
    {
        get => _margin;
        set => SetProperty(ref _margin, value);
    }

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public int ChildCount { get; set; }

    public int CaretIndex
    {
        get => _caretIndex;
        set => SetProperty(ref _caretIndex, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public int SelectionLength
    {
        get => _selectionLength;
        set => SetProperty(ref _selectionLength, value);
    }

    public override string ToString()
    {
        return $"{Index}, {Name}, {IsSelected}";
    }
}