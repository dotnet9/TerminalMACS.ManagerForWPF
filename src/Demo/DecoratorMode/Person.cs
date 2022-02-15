using System;

namespace DecoratorMode;

internal class Person
{
    private readonly string name;

    public Person()
    {
    }

    public Person(string name)
    {
        this.name = name;
    }

    public virtual void Show()
    {
        Console.WriteLine($"装扮的{name}");
    }
}