using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop.Utilities;
using WpfAppForZoomInAndZoomOut.Models;
using WpfAppForZoomInAndZoomOut.ViewModels;

namespace WpfAppForZoomInAndZoomOut;

public partial class ChildView1 : UserControl
{
    public ChildView1ViewModel? ViewModel
    {
        get => this.DataContext as ChildView1ViewModel;
        set => this.DataContext = value;
    }
    public ChildView1()
    {
        this.ViewModel = new ChildView1ViewModel();
        InitializeComponent();
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
}