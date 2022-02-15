using System;

namespace StateMode._4;

/// <summary>
///     下班休息
/// </summary>
internal class RestState : State
{
    public override void WriteProgram(Work w)
    {
        Console.WriteLine($"当前时间：{w.Hour}点下班回家了");
    }
}