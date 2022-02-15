using System;

namespace TheMementoPattern._2;

/// <summary>
///     发起人类
/// </summary>
internal class Originator
{
    /// <summary>
    ///     需要保存的属性，可能有多个
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///     创建备忘录，将当前需要保存的信息导入实例化出一个Memento对象
    /// </summary>
    /// <returns></returns>
    public Memento CreateMemento()
    {
        return new Memento(State);
    }

    /// <summary>
    ///     恢复备忘录，将Memento导入并将相关数据恢复
    /// </summary>
    /// <param name="memento"></param>
    public void SetMemento(Memento memento)
    {
        State = memento.State;
    }

    public void Show()
    {
        Console.WriteLine($"State={State}");
    }
}