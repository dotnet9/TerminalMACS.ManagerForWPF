using System;

namespace StateMode._4;

/// <summary>
///     中午工作状态类
/// </summary>
public class NoonState : State
{
    public override void WriteProgram(Work w)
    {
        if (w.Hour < 13)
        {
            Console.WriteLine($"当前时间:{w.Hour}点，饿了，午饭：犯困，午休。");
        }
        else
        {
            /// 超过13点,则转入下午工作状态
            w.SetState(new AfternoonState());
            w.WriteProgram();
        }
    }
}