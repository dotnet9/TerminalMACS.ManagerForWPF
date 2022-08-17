using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfAppForZoomInAndZoomOut.Models;

namespace WpfAppForZoomInAndZoomOut.ViewModels
{
    public class ChildView1ViewModel : BindableBase, IDropTarget
    {
        private readonly ObservableCollection<TreeItemModel> _itemSource = new();
        private const int MoveStep = 5;
        private TreeItemModel? _dragTargetItem;
        private int _dragInsertPosition;
        private Point _sourcePosition;
        private Point _targetPosition;

        public TreeItemModel? DragTargetItem
        {
            get => this._dragTargetItem;
            set => this.SetProperty(ref this._dragTargetItem, value);
        }

        public int DragInsertPosition
        {
            get => this._dragInsertPosition;
            set => this.SetProperty(ref this._dragInsertPosition, value);
        }

        public Point SourcePosition
        {
            get => this._sourcePosition;
            set => this.SetProperty(ref this._sourcePosition, value);
        }

        public Point TargetPosition
        {
            get => this._targetPosition;
            set => this.SetProperty(ref this._targetPosition, value);
        }

        public ChildView1ViewModel()
        {
            const int listCount = 20;
            for (var i = 0; i < listCount; i++)
                _itemSource.Add(new TreeItemModel
                {
                    Index = i + 1, Name = $"测试{i}", ChildCount = Random.Shared.Next(0, 5),
                    Margin = new Thickness(Random.Shared.Next(10) * MoveStep, 0, 0, 0)
                });
        }

        public ObservableCollection<TreeItemModel> ItemSource
        {
            get => this._itemSource;
        }

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

                this.DragTargetItem = dropInfo.TargetItem as TreeItemModel;
                this.SourcePosition = dropInfo.DropPosition;
                this.TargetPosition = dropInfo.DragInfo.DragStartPosition;
                this.DragInsertPosition = (int)((dropInfo.DropPosition.X - dropInfo.DragInfo.DragStartPosition.X) / 5);
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

            var sourceItemIndex = this._itemSource.IndexOf(sourceItem);
            var targetItemIndex = this._itemSource.IndexOf(targetItem);

            var left = targetItem.Margin.Left;
            left += MoveStep * this.DragInsertPosition;

            sourceItem.Margin = new Thickness(left, 0, 0, 0);
            this._itemSource.Move(sourceItemIndex,
                sourceItemIndex > targetItemIndex ? (targetItemIndex + 1) : targetItemIndex);
            UpdatePage();
            DragTargetItem = null;
        }

        private void UpdatePage()
        {
            for (var i = 0; i < _itemSource.Count; i++)
            {
                _itemSource[i].Index = i + 1;
            }
        }
    }
}