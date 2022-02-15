namespace AgentModel;

/// <summary>
///     代理
/// </summary>
internal class Proxy : GiveGift
{
    private readonly Pursuit gg;

    public Proxy(SchoolGirl mm)
    {
        gg = new Pursuit(mm);
    }

    public void GiveDolls()
    {
        gg.GiveDolls();
    }

    public void GiveFlowers()
    {
        gg.GiveFlowers();
    }

    public void GiveChocolate()
    {
        gg.GiveChocolate();
    }
}