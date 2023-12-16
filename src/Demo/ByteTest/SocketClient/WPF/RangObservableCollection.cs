namespace SocketClient.WPF;

public class RangObservableCollection<T> : ObservableCollection<T>
{
    private bool SuppressNotification { get; set; } = false;

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!SuppressNotification)
        {
            base.OnCollectionChanged(e);
        }
    }

    public new void Clear()
    {
        Items.Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void AddRange(IEnumerable<T> collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        SuppressNotification = true;

        foreach (var item in collection)
        {
            Items.Add(item);
        }

        SuppressNotification = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}