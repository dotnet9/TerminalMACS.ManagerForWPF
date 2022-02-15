using System;

namespace TheMementoPattern._3;

/// <summary>
///     游戏角色类，用来存储角色的生命力、攻击力、防御力的数据
/// </summary>
internal class GameRole
{
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

    /// <summary>
    ///     状态显示
    /// </summary>
    public void StateDisplay()
    {
        Console.WriteLine($"角色当前状态：\r\n体力：{Vitality}\r\n攻击力：{Attack}\r\n防御力：{Defense}");
    }

    /// <summary>
    ///     获得初始状态
    /// </summary>
    public void GetInitState()
    {
        // 数据通常来自本机磁盘或远程数据库
        Vitality = 100;
        Attack = 100;
        Defense = 100;
    }

    /// <summary>
    ///     战斗
    /// </summary>
    public void Fight()
    {
        // 在与Boss大战后游戏数据损耗为零
        Vitality = 0;
        Attack = 0;
        Defense = 0;
    }

    /// <summary>
    ///     保存角色状态，新增“保存角色状态”方法，将游戏角色的三个状态值通过实例化“角色状态存储箱”返回
    /// </summary>
    /// <returns></returns>
    public RoleStateMemento SaveState()
    {
        return new RoleStateMemento(Vitality, Attack, Defense);
    }

    /// <summary>
    ///     恢复角色状态，新增“恢复角色状态”方法，可将外部的“角色状态存储箱”中状态值恢复给游戏角色
    /// </summary>
    /// <param name="memento"></param>
    public void RecoveryState(RoleStateMemento memento)
    {
        Vitality = memento.Vitality;
        Attack = memento.Attack;
        Defense = memento.Defense;
    }
}