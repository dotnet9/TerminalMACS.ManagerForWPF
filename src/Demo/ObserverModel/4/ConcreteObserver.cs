using System;

namespace ObserverModel._4;

internal class ConcreteObserver : Observer
{
    private readonly string name;
    private string observerState;

    public ConcreteObserver(ConcreteSubject subject, string name)
    {
        this.Subject = subject;
        this.name = name;
    }

    public ConcreteSubject Subject { get; set; }

    public override void Update()
    {
        observerState = Subject.SubjectState;
        Console.WriteLine($"观察者{name}的新状态是{observerState}");
    }
}