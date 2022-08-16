using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using WpfAppForZoomInAndZoomOut.Models;

namespace WpfAppForZoomInAndZoomOut.ViewModels
{
    public class ChildView1ViewModel : BindableBase, IDropTarget
    {
        private ObservableCollection<TestModel> _itemSource = new();
        private const int moveStep = 5;

        public ChildView1ViewModel()
        {
            const int listCount = 100;
            for (var i = 0; i < listCount; i++)
                _itemSource.Add(new TestModel
                {
                    Index = i + 1, Name = $"测试{i}", ChildCount = Random.Shared.Next(0, 5),
                    Margin = new Thickness(Random.Shared.Next(10) * moveStep, 0, 0, 0)
                });
        }

        public ObservableCollection<TestModel> ItemSource
        {
            get => this._itemSource;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is not TestModel sourceItem || dropInfo.TargetItem is not TestModel targetItem) return;

            if (sourceItem == targetItem)
            {
                dropInfo.Effects = DragDropEffects.None;
                return;
            }

            var sourceItemIndex = this._itemSource.IndexOf(sourceItem);
            var targetItemIndex = this._itemSource.IndexOf(targetItem);

            if (Math.Abs(sourceItemIndex - targetItemIndex) == 1 &&
                Math.Abs(sourceItem.Margin.Left - targetItem.Margin.Left) < 1)
            {
                dropInfo.Effects = DragDropEffects.None;
                return;
            }

            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            if (dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.AfterTargetItem))
            {
                dropInfo.EffectText = "缩进";
            }
            else if (dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.BeforeTargetItem))
            {
                dropInfo.EffectText = "减少缩进";
            }
            else
            {
                dropInfo.EffectText = "插入";
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is not TestModel sourceItem || dropInfo.TargetItem is not TestModel targetItem) return;
            if (sourceItem == targetItem)
            {
                return;
            }

            var sourceItemIndex = this._itemSource.IndexOf(sourceItem);
            var targetItemIndex = this._itemSource.IndexOf(targetItem);

            var left = targetItem.Margin.Left;
            if (dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.AfterTargetItem))
            {
                left += moveStep;
            }
            else if (dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.BeforeTargetItem))
            {
                left -= moveStep;
            }

            sourceItem.Margin = new Thickness(left, 0, 0, 0);
            this._itemSource.Move(sourceItemIndex,
                sourceItemIndex > targetItemIndex ? (targetItemIndex + 1) : targetItemIndex);
            UpdatePage();
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