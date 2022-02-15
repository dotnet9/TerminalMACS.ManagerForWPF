using System.Collections.Generic;

namespace IteratorModel._1;

/// <summary>
///     具体聚集类
/// </summary>
internal class ConcreteAggregate : Aggregate
{
    // 声明一个IList泛型变量，用于存放聚合对象，用ArrayList同样可以实现
    private readonly IList<object> items = new List<object>();

    /// <summary>
    ///     返回聚集总个数
    /// </summary>
    public int Count => items.Count;

    /// <summary>
    ///     声明一个索引器
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public object this[int index]
    {
        get => items[index];
        set => items.Insert(index, value);
    }

    public override Iterator CreateIterator()
    {
        return new ConcreteIterator(this);
    }
}