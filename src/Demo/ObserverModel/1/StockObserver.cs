using System;

namespace ObserverModel._1;

/// <summary>
///     看股票同事类
/// </summary>
internal class StockObserver
{
    private readonly string name;
    private readonly Secretary sub;

    public StockObserver(string name, Secretary sub)
    {
        this.name = name;
        this.sub = sub;
    }

    public void Update()
    {
        Console.WriteLine($"{sub.SecretaryAction}{name}关闭股票行情，继续工作! ");
    }
}