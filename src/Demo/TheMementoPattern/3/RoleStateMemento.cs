namespace TheMementoPattern._3;

/// <summary>
///     角色状态存储箱
/// </summary>
internal class RoleStateMemento
{
    /// <summary>
    ///     将生命力、攻击力、防御力存入状态存储箱对象中
    /// </summary>
    /// <param name="vitality"></param>
    /// <param name="attack"></param>
    /// <param name="defense"></param>
    public RoleStateMemento(int vitality, int attack, int defense)
    {
        Vitality = vitality;
        Attack = attack;
        Defense = defense;
    }

    /// <summary>
    ///     生命力
    /// </summary>
    public int Vitality { get; set; }

    /// <summary>
    ///     攻击力
    /// </summary>
    public int Attack { get; set; }

    /// <summary>
    ///     防御力
    /// </summary>
    public int Defense { get; set; }
}