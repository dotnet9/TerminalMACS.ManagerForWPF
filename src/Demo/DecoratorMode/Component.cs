using System;

namespace DecoratorMode;

internal abstract class Component
{
    public abstract void Operation();
}

internal class ConcreteComponent : Component
{
    public override void Operation()
    {
        Console.WriteLine("具体对象的操作");
    }
}

internal abstract class Decorator : Component
{
    protected Component component;

    public void SetComponent(Component component)
    {
        this.component = component;
    }

    /// <summary>
    ///     重写Operation)，实际执行的是Component的 Operation()
    /// </summary>
    public override void Operation()
    {
        if (component != null) component.Operation();
    }
}

internal class ConcreteDecoratorA : Decorator
{
    // 本类的独有功能，以区别于ConcreteDecoratorB
    private string addedstate;

    public override void Operation()
    {
        //首先运行原Component的Operation(，再执行本类的功能，如addedState，相当于对原Component进行了装饰
        base.Operation();
        addedstate = "New State";
        Console.WriteLine("具体装饰对象A的操作");
    }
}

internal class ConcreteDecoratorB : Decorator
{
    // 首先运行原 Component的 Operation(), 再执行本类的功能，如AddedBehavior(，相当于对原Component进行了装饰
    public override void Operation()
    {
        base.Operation();

        AddedBehavior();
        Console.WriteLine("具体装饰对象B的操作");
    }

    // 本类独有的方法，以区别于ConcreteDecoratorB
    private void AddedBehavior()
    {
    }
}