namespace IteratorModel._1;

internal class ConcreteIterator : Iterator
{
    // 定义了一个具体聚集对象
    private readonly ConcreteAggregate aggregate;
    private int current;

    /// <summary>
    ///     初始化时将具体的聚集对象传入
    /// </summary>
    /// <param name="aggregate"></param>
    public ConcreteIterator(ConcreteAggregate aggregate)
    {
        this.aggregate = aggregate;
    }

    public override object First()
    {
        return aggregate[0];
    }

    public override object Next()
    {
        object ret = null;

        current++;
        if (current < aggregate.Count) ret = aggregate[current];

        return ret;
    }

    public override bool IsDone()
    {
        return current >= aggregate.Count;
    }

    /// <summary>
    ///     返回当前的聚集对象
    /// </summary>
    /// <returns></returns>
    public override object CurrentItem()
    {
        return aggregate[current];
    }
}