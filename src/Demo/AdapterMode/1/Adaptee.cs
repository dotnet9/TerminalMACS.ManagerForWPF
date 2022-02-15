using System;

namespace AdapterMode._1;

internal class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("特殊请求");
    }
}