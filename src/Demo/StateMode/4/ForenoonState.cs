using System;

namespace StateMode._4
{
  /// <summary>
  /// 上午工作状态类
  /// </summary>
  public class ForenoonState : State
  {
    public override void WriteProgram(Work w)
    {
      if (w.Hour < 12)
      {
        Console.WriteLine($"当前时间:{w.Hour}点上午工作，精神百倍");
      }
      else
      {
        /// 超过12点,则转入中午工作状态
        w.SetState(new NoonState());
        w.WriteProgram();
      }
    }
  }
}
