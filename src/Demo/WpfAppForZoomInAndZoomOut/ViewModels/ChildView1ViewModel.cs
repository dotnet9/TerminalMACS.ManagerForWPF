using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Prism.Mvvm;
using WpfAppForZoomInAndZoomOut.Models;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class ChildView1ViewModel : BindableBase //, IDropTarget
{
    private bool isDraging;
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

    public void DragStart(object sender, MouseButtonEventArgs e)
    {
        var parent = (ListBox)sender;
        SourcePosition = e.GetPosition(parent);
        var hitTestControl = VisualTreeHelper.HitTest(parent, SourcePosition);

        if (hitTestControl is
            { VisualHit: Path { Name: "Path_DragPosition" } or Border { Name: "Border_DragPosition" } })
        {
            DragSourceItem =
                GetDataFromListBox(parent, SourcePosition) as TreeItemModel;
            if (DragSourceItem != null)
            {
                isDraging = true;
                DragDrop.DoDragDrop(parent, DragSourceItem, DragDropEffects.Move);
            }
            else
            {
                ClearDragInfo();
            }
        }
        else
        {
            ClearDragInfo();
        }
    }

    public void DragOver(object sender, DragEventArgs e)
    {
        if (!isDraging) return;

        var parent = (ListBox)sender;
        TargetPosition = e.GetPosition(parent);
        var targetItem = GetDataFromListBox(parent, TargetPosition) as TreeItemModel;
        if (DragSourceItem != targetItem)
        {
            this.DragTargetItem = targetItem;
            e.Effects = DragDropEffects.Move;
            DragInsertPosition = (int)((TargetPosition.X - SourcePosition.X) / 5);
        }
        else
        {
            this.DragTargetItem = null;
            e.Effects = DragDropEffects.None;
        }
    }

    public void Drop(object sender, DragEventArgs e)
    {
        var parent = (ListBox)sender;

        if (!isDraging || DragSourceItem == null || DragTargetItem == null) return;


        var sourceItemIndex = ItemSource.IndexOf(DragSourceItem);
        var targetItemIndex = ItemSource.IndexOf(DragTargetItem);

        var left = DragTargetItem.Margin.Left;
        left += ChildView1ViewModel.MoveStep * DragInsertPosition;

        DragSourceItem.Margin = new Thickness(left, 0, 0, 0);
        ItemSource.Move(sourceItemIndex,
            sourceItemIndex > targetItemIndex ? targetItemIndex + 1 : targetItemIndex);
        UpdatePage();

        ClearDragInfo();
    }


    private static object? GetDataFromListBox(ItemsControl source, Point point)
    {
        if (source.InputHitTest(point) is not UIElement hitTestElement) return null;
        var element = hitTestElement;
        var data = DependencyProperty.UnsetValue;
        while (data == DependencyProperty.UnsetValue)
        {
            data = source.ItemContainerGenerator.ItemFromContainer(element!);

            if (data == DependencyProperty.UnsetValue) element = VisualTreeHelper.GetParent(element!) as UIElement;

            if (element == source) return null;
        }

        return data != DependencyProperty.UnsetValue ? data : null;
    }

    public void ClearDragInfo()
    {
        isDraging = false;
        this.DragTargetItem = null;
    }
}