using System.Collections.Generic;

namespace ObserverModel._2;

/// <summary>
///     前台秘书类
/// </summary>
internal class Secretary
{
    // 同事列表
    private readonly IList<Observer> observers = new List<Observer>();

    //前台状态
    public string SecretaryAction { get; set; }


    // 增加
    // 针对抽象编程,减少了与具体类的耦合
    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }


    // 减少
    // 针对抽象编程,减少了与具体类的耦合
    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    //通知
    public void Notify()
    {
        foreach (var o in observers) o.Update();
    }
}