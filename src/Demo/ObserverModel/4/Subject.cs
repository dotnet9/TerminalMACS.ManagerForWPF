using System.Collections.Generic;

namespace ObserverModel._4;

internal abstract class Subject
{
    private readonly IList<Observer> observers = new List<Observer>();

    //增加观察者
    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    //移除观察者
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