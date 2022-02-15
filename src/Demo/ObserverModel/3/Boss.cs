using System.Collections.Generic;

namespace ObserverModel._3;

internal class Boss : Subject
{
    // 同事列表
    private readonly IList<Observer> observers = new List<Observer>();

    //增加
    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    //减少
    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    //通知
    public void Notify()
    {
        foreach (var o in observers) o.Update();
    }

    //老板状态
    public string SubjectState { get; set; }
}