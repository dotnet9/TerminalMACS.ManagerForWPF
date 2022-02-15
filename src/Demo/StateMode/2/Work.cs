using System;

namespace StateMode._2;

internal class Work
{
    public int Hour { get; set; }
    public bool TaskFinished { get; set; }


    public void WriteProgram()
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
            if (TaskFinished)
            {
                Console.WriteLine("当前时间:{0}点下班回家了", Hour);
            }
            else
            {
                if (Hour < 21)
                    Console.WriteLine("当前时间:{0}点加班哦，疲累之极", Hour);
                else
                    Console.WriteLine("当前时间:{0}点不行了，睡着了。", Hour);
            }
        }
    }
}