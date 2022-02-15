using System;

namespace StateMode._4;

/// <summary>
///     下午和傍晚工作状态类
/// </summary>
public class AfternoonState : State
{
    public override void WriteProgram(Work w)
    {
        if (w.Hour < 17)
        {
            Console.WriteLine($"当前时间:{w.Hour}点，下午状态还不错，继续努力。");
        }
        else
        {
            /// 超过17点,则转入傍晚工作状态
            w.SetState(new EveningState());
            w.WriteProgram();
        }
    }
}