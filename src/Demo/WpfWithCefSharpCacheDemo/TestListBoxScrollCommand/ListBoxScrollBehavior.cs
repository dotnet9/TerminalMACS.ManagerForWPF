using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfWithCefSharpCacheDemo.TestListBoxScrollCommand;

public class ListBoxScrollBehavior : Behavior<ListBox>
{
    public ICommand ScrollCommand
    {
        get { return (ICommand)GetValue(ScrollCommandProperty); }
        set { SetValue(ScrollCommandProperty, value); }
    }

    public static readonly DependencyProperty ScrollCommandProperty =
        DependencyProperty.Register(nameof(ScrollCommand), typeof(ICommand), typeof(ListBoxScrollBehavior),
            new PropertyMetadata(null));

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(OnScrollChanged));
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(OnScrollChanged));
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        ScrollCommand.Execute(e);
    }
}