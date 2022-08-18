using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfAppForZoomInAndZoomOut.Models;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut;

public partial class ChildView1 : UserControl
{
    private bool isDraging;

    public ChildView1()
    {
        ViewModel = new ChildView1ViewModel();
        InitializeComponent();
    }

    public ChildView1ViewModel? ViewModel
    {
        get => DataContext as ChildView1ViewModel;
        set => DataContext = value;
    }


    private void NewSelectedItemChanged(object sender, KeyboardFocusChangedEventArgs e)
    {
        //var item = (ListBoxItem)sender;
        //var testModel = (TestModel)item.DataContext;
        //this.ViewModel!.ItemSource.Where(x => !x.Equals(testModel)).ToList().ForEach(x => x.IsSelected = false);
        //testModel.IsSelected = true;
    }

    private void TestListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control) return;

        var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArgs.RoutedEvent = MouseWheelEvent;
        eventArgs.Source = sender;
        ((FrameworkElement)sender).RaiseEvent(eventArgs);
    }

    //private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    //{
    //    var textBox = (TextBox)sender;
    //    if (textBox.SelectedText.Length > 0)
    //    {
    //        Popup.PlacementTarget = textBox;
    //        Popup.IsOpen = true;
    //    }
    //    else
    //    {
    //        Popup.IsOpen = false;
    //    }
    //}

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        //Popup.IsOpen = false;
    }

    private void TestListBoxIns_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var parent = (ListBox)sender;
        ViewModel!.SourcePosition = e.GetPosition(parent);
        var hitTestControl = VisualTreeHelper.HitTest(parent, ViewModel!.SourcePosition);

        if (hitTestControl is
            { VisualHit: Path { Name: "Path_DragPosition" } or Border { Name: "Border_DragPosition" } })
        {
            ViewModel!.DragSourceItem =
                GetDataFromListBox(parent, ViewModel!.SourcePosition) as TreeItemModel;
            if (ViewModel!.DragSourceItem != null)
            {
                isDraging = true;
                DragDrop.DoDragDrop(parent, ViewModel!.DragSourceItem, DragDropEffects.Move);
            }
            else
            {
                isDraging = false;
            }
        }
        else
        {
            isDraging = false;
        }
    }

    private void TestListBoxIns_OnDragOver(object sender, DragEventArgs e)
    {
        if (!isDraging) return;

        var parent = (ListBox)sender;
        ViewModel!.TargetPosition = e.GetPosition(parent);
        ViewModel!.DragTargetItem = GetDataFromListBox(parent, ViewModel!.TargetPosition) as TreeItemModel;
        ViewModel!.DragInsertPosition = (int)((ViewModel!.TargetPosition.X - ViewModel!.SourcePosition.X) / 5);
    }

    private void TestListBoxIns_OnDrop(object sender, DragEventArgs e)
    {
        var parent = (ListBox)sender;

        if (!isDraging || ViewModel!.DragSourceItem == null || ViewModel!.DragTargetItem == null) return;


        var sourceItemIndex = ViewModel.ItemSource.IndexOf(ViewModel!.DragSourceItem);
        var targetItemIndex = ViewModel.ItemSource.IndexOf(ViewModel!.DragTargetItem);

        var left = ViewModel!.DragTargetItem.Margin.Left;
        left += ChildView1ViewModel.MoveStep * ViewModel!.DragInsertPosition;

        ViewModel!.DragSourceItem.Margin = new Thickness(left, 0, 0, 0);
        ViewModel.ItemSource.Move(sourceItemIndex,
            sourceItemIndex > targetItemIndex ? targetItemIndex + 1 : targetItemIndex);
        ViewModel.UpdatePage();
        ViewModel.DragTargetItem = null;

        isDraging = false;
    }

    private static object GetDataFromListBox(ListBox source, Point point)
    {
        var element = source.InputHitTest(point) as UIElement;
        if (element != null)
        {
            var data = DependencyProperty.UnsetValue;
            while (data == DependencyProperty.UnsetValue)
            {
                data = source.ItemContainerGenerator.ItemFromContainer(element);

                if (data == DependencyProperty.UnsetValue) element = VisualTreeHelper.GetParent(element) as UIElement;

                if (element == source) return null;
            }

            if (data != DependencyProperty.UnsetValue) return data;
        }

        return null;
    }
}