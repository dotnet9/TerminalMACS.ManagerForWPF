using System;

namespace AgentModel;

/// <summary>
///     追求者
/// </summary>
internal class Pursuit : GiveGift
{
    private readonly SchoolGirl mm;

    public Pursuit(SchoolGirl mm)
    {
        this.mm = mm;
    }

    public void GiveDolls()
    {
        Console.WriteLine(mm.Name + "送你洋娃娃");
    }

    public void GiveFlowers()
    {
        Console.WriteLine(mm.Name + "送你鲜花");
    }

    public void GiveChocolate()
    {
        Console.WriteLine(mm.Name + "送你巧克力");
    }
}