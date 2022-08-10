using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfAppForZoomInAndZoomOut;

public partial class ChildView1 : UserControl
{
    private readonly List<TestModel> _itemSource = new();

    public ChildView1()
    {
        InitializeComponent();
        var listCount = 100;
        for (var i = 0; i < listCount; i++) _itemSource.Add(new TestModel { Index = i, Name = Helper.RandomString() });

        TestListBoxIns.ItemsSource = _itemSource;
        TxtShowListBoxInfo.Text = $"列表共有{listCount}条数据";
    }


    private void NewSelectedItemChanged(object sender, KeyboardFocusChangedEventArgs e)
    {
        var item = (ListBoxItem)sender;
        var contenxt = (TestModel)item.DataContext;
        _itemSource.Where(x => !x.Equals(contenxt)).ToList().ForEach(x => x.IsSelected = false);
        contenxt.IsSelected = false;
    }

    private void TestListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control) return;

        var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArgs.RoutedEvent = MouseWheelEvent;
        eventArgs.Source = sender;
        ((FrameworkElement)sender).RaiseEvent(eventArgs);
    }
}