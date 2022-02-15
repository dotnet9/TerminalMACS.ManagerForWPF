namespace StateMode._3;

internal class ConcreteStateA : State
{
    /// <summary>
    ///     设置ConcreteStateA的下一状态是 ConcreteStateB
    /// </summary>
    /// <param name="context"></param>
    public override void Handle(Context context)
    {
        context.State = new ConcreteStateB();
    }
}

internal class ConcreteStateB : State
{
    /// <summary>
    ///     设置ConcreteStateB的下一状态是 ConcreteStateA
    /// </summary>
    /// <param name="context"></param>
    public override void Handle(Context context)
    {
        context.State = new ConcreteStateA();
    }
}