using System;

namespace DecoratorMode;

internal class Finery : Person
{
    protected Person component;

    // 打扮
    public void Decorate(Person component)
    {
        this.component = component;
    }

    public override void Show()
    {
        if (component != null) component.Show();
    }
}

internal class TShirts : Finery
{
    public override void Show()
    {
        Console.Write("大T恤");
        base.Show();
    }
}

internal class BigTrouser : Finery
{
    public override void Show()
    {
        Console.Write("垮裤");
        base.Show();
    }
}