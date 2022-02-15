using IteratorModel._1;

namespace IteratorModel._2;

internal class ConcreteIteratorDesc : Iterator
{
    // 定义了一个具体聚集对象
    private readonly ConcreteAggregate aggregate;
    private int current;

    /// <summary>
    ///     初始化时将具体的聚集对象传入
    /// </summary>
    /// <param name="aggregate"></param>
    public ConcreteIteratorDesc(ConcreteAggregate aggregate)
    {
        this.aggregate = aggregate;
        current = aggregate.Count - 1;
    }

    /// <summary>
    ///     得到聚焦的第一个对象
    /// </summary>
    /// <returns></returns>
    public override object First()
    {
        return aggregate[aggregate.Count - 1];
    }

    /// <summary>
    ///     得到聚焦的下一个对象
    /// </summary>
    /// <returns></returns>
    public override object Next()
    {
        object ret = null;

        current--;
        if (current >= 0) ret = aggregate[current];

        return ret;
    }

    /// <summary>
    ///     判断当前是否遍历到结尾，到结尾返回true
    /// </summary>
    /// <returns></returns>
    public override bool IsDone()
    {
        return current < 0;
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