using System;

namespace StateMode._4;

/// <summary>
///     晚间工作状态
/// </summary>
public class EveningState : State
{
    public override void WriteProgram(Work w)
    {
        if (w.TaskFinished)
        {
            // 如果完成任务，则转入下班状态
            w.SetState(new RestState());
            w.WriteProgram();
        }
        else
        {
            if (w.Hour < 21)
            {
                Console.WriteLine($"当前时间：{w.Hour}点，加班哦，疲累之极");
            }
            else
            {
                //超过21点，则转入睡眠工作状态
                w.SetState(new SleepingState());
                w.WriteProgram();
            }
        }
    }
}