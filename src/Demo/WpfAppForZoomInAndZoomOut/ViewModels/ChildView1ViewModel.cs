using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using WpfAppForZoomInAndZoomOut.Models;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class ChildView1ViewModel : BindableBase, IDropTarget
{
    private const int MoveStep = 5;
    private int _dragInsertPosition;
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

    public void DragEnter(IDropInfo dropInfo)
    {
        Debug.Print("DragEnter");
    }

    public void DragOver(IDropInfo dropInfo)
    {
        Debug.Print("DragOver");
        var result = VisualTreeHelper.HitTest(dropInfo.VisualTarget, dropInfo.DragInfo.DragStartPosition);

        if (result is { VisualHit: Path { Name: "Path_DragPosition" } or Border { Name: "Border_DragPosition" } })
        {
            Debug.Print("draging");

            DragTargetItem = dropInfo.TargetItem as TreeItemModel;
            SourcePosition = dropInfo.DropPosition;
            TargetPosition = dropInfo.DragInfo.DragStartPosition;
            DragInsertPosition = (int)((dropInfo.DropPosition.X - dropInfo.DragInfo.DragStartPosition.X) / 5);
            dropInfo.Effects = DragDropEffects.Copy | DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        }
        else
        {
            Debug.Print($"Can't drag, the hit test visual is {(result.VisualHit as FrameworkElement)?.Name}");
            dropInfo.Effects = DragDropEffects.None;
        }
    }

    public void DragLeave(IDropInfo dropInfo)
    {
        Debug.Print("DragLeave");
        DragTargetItem = null;
    }

    public void Drop(IDropInfo dropInfo)
    {
        Debug.Print("Drop");

        if (dropInfo.Data is not TreeItemModel sourceItem ||
            dropInfo.TargetItem is not TreeItemModel targetItem) return;

        Debug.Print("Drop-busy");

        var sourceItemIndex = ItemSource.IndexOf(sourceItem);
        var targetItemIndex = ItemSource.IndexOf(targetItem);

        var left = targetItem.Margin.Left;
        left += MoveStep * DragInsertPosition;

        sourceItem.Margin = new Thickness(left, 0, 0, 0);
        ItemSource.Move(sourceItemIndex,
            sourceItemIndex > targetItemIndex ? targetItemIndex + 1 : targetItemIndex);
        UpdatePage();
        DragTargetItem = null;
    }

    private void UpdatePage()
    {
        for (var i = 0; i < ItemSource.Count; i++) ItemSource[i].Index = i + 1;
    }
}