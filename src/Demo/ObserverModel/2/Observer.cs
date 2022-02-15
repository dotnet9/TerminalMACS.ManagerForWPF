namespace ObserverModel._2;

/// <summary>
///     抽象的观察者
/// </summary>
internal abstract class Observer
{
    protected string name;
    protected Secretary sub;

    public Observer(string name, Secretary sub)
    {
        this.name = name;
        this.sub = sub;
    }

    public abstract void Update();
}