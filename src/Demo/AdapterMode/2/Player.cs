namespace AdapterMode._2;

/// <summary>
///     球员
/// </summary>
internal abstract class Player
{
    protected string name;

    public Player(string name)
    {
        this.name = name;
    }

    // 进攻和防守方法
    public abstract void Attack();
    public abstract void Defense();
}