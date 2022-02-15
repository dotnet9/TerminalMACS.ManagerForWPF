using System;

namespace AdapterMode._1;

internal class Target
{
    public virtual void Request()
    {
        Console.WriteLine("普通请求");
    }
}