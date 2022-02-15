using System;

namespace ObserverModel._2;

/// <summary>
///     看股票的同事
/// </summary>
internal class StockObserver : Observer
{
    public StockObserver(string name, Secretary sub) : base(name, sub)
    {
    }

    public override void Update()
    {
        Console.WriteLine($"{sub.SecretaryAction} {name}关闭股票行情，继续工作! ");
    }
}