using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prism.Mvvm;
using WpfAppForZoomInAndZoomOut.Models;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class ChildView1ViewModel : BindableBase //, IDropTarget
{
    public const int MoveStep = 5;
    private int _dragInsertPosition;
    private TreeItemModel? _dragSourceItem;
    private TreeItemModel? _dragTargetItem;
    private Point _sourcePosition;
    private Point _targetPosition;

    public ChildView1ViewModel()
    {
        const int listCount = 20;
        for (var i = 0; i < listCount; i++)
            ItemSource.Add(new TreeItemModel
            {
                Index = i + 1, Name = $"测试{i}", ChildCount = Random.Shared.Next(0, 5),
                Margin = new Thickness(Random.Shared.Next(10) * MoveStep, 0, 0, 0)
            });
    }

    public TreeItemModel? DragSourceItem
    {
        get => _dragSourceItem;
        set => SetProperty(ref _dragSourceItem, value);
    }

    public TreeItemModel? DragTargetItem
    {
        get => _dragTargetItem;
        set => SetProperty(ref _dragTargetItem, value);
    }

    public int DragInsertPosition
    {
        get => _dragInsertPosition;
        set => SetProperty(ref _dragInsertPosition, value);
    }

    public Point SourcePosition
    {
        get => _sourcePosition;
        set => SetProperty(ref _sourcePosition, value);
    }

    public Point TargetPosition
    {
        get => _targetPosition;
        set => SetProperty(ref _targetPosition, value);
    }

    public ObservableCollection<TreeItemModel> ItemSource { get; } = new();


    public void UpdatePage()
    {
        for (var i = 0; i < ItemSource.Count; i++) ItemSource[i].Index = i + 1;
    }
}