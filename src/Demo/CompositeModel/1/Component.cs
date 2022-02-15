namespace CompositeModel._1;

internal abstract class Component
{
    protected string name;

    public Component(string name)
    {
        this.name = name;
    }

    // 通常都用Add和Remove方法来提供增加或移除树叶或树枝的功能
    public abstract void Add(Component c);
    public abstract void Remove(Component c);
    public abstract void Display(int depth);
}