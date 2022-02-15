using System;

namespace StateMode._4;

/// <summary>
///     睡眠状态
/// </summary>
public class SleepingState : State
{
    public override void WriteProgram(Work w)
    {
        Console.WriteLine($"当前时间：{w.Hour}点不行了，睡着了。");
    }
}