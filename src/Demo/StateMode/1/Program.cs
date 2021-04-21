using System;

namespace StateMode._1
{
  class Program
  {
    static int Hour = 0;//钟点
    static bool WorkFinished = false;//任务完成标记//写程序方法

    /// <summary>
    /// 定义一个“写程序”的函数，用来根据时间的不同体现不同的工作状态
    /// </summary>
    public static void WriteProgram()
    {

      if (Hour < 12)
      {
        Console.WriteLine("当前时间:{0}点上午工作，精神百倍", Hour);
      }
      else if (Hour < 13)
      {
        Console.WriteLine("当前时间:{0}点饿了，午饭;犯困，午休。", Hour);
      }
      else if (Hour < 17)
      {
        Console.WriteLine("当前时间:{0}点下午状态还不错，继续努力", Hour);
      }
      else
      {
        if (WorkFinished)
        {
          Console.WriteLine("当前时间:{0}点下班回家了", Hour);
        }
        else
        {
          if (Hour < 21)
          {
            Console.WriteLine("当前时间:{0}点加班哦，疲累之极", Hour);
          }
          else
          {
            Console.WriteLine("当前时间:{0}点不行了，睡着了。", Hour);
          }

        }
      }
    }

    //static void Main(string[] args)
    //{
    //  Hour = 9;
    //  WriteProgram();

    //  Hour = 10;
    //  WriteProgram();

    //  Hour = 12;
    //  WriteProgram();

    //  Hour = 13;
    //  WriteProgram();

    //  Hour = 14;
    //  WriteProgram();

    //  Hour = 17;

    //  // 任务完成,则可以下班,否则就得加班了
    //  WorkFinished = true;
    //  //workFinished = false;

    //  WriteProgram();

    //  Hour = 19;
    //  WriteProgram();

    //  Hour = 22;
    //  WriteProgram();

    //  Console.Read();
    //}
  }
}
