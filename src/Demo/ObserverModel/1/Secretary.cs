using System.Collections.Generic;

namespace ObserverModel._1;

/// <summary>
///     前台秘书类
/// </summary>
internal class Secretary
{
    //同事列表
    private readonly IList<StockObserver> observers = new List<StockObserver>();

    // 前台状态
    // 前台通过电话,所说的话或所做的事
    public string SecretaryAction { get; set; }

    // 增加
    // 就是有几个同事请前台帮忙，于是就给集合增加几个对象
    public void Attach(StockObserver observer)
    {
        observers.Add(observer);
    }

    // 通知
    // 待老板来时，就给所有的登记的同事们发通知,“老板来了”
    public void Notify()
    {
        foreach (var o in observers) o.Update();
    }
}