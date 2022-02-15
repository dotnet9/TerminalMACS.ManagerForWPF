namespace IteratorModel._1;

/// <summary>
///     聚集抽象类
/// </summary>
internal abstract class Aggregate
{
    /// <summary>
    ///     创建迭代器
    /// </summary>
    /// <returns></returns>
    public abstract Iterator CreateIterator();
}