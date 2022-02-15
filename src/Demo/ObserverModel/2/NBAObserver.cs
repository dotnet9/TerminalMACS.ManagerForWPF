using System;

namespace ObserverModel._2;

/// <summary>
///     看NBA的同事
/// </summary>
internal class NBAObserver : Observer
{
    public NBAObserver(string name, Secretary sub) : base(name, sub)
    {
    }

    public override void Update()
    {
        Console.WriteLine($"{sub.SecretaryAction} {name}关闭NBA直播，继续工作! ");
    }
}