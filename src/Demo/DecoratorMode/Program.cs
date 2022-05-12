using System;

namespace DecoratorMode;

internal class Test
{
    public static void NewMethod2()
    {
        var templ = Temp.Value1;
        var temp2 = Temp.Value2;
        Console.WriteLine(templ == temp2);
        Console.WriteLine(templ.Equals(temp2));
        Console.WriteLine(templ.CompareTo(temp2));
        Console.WriteLine(templ == Temp.Value1);
        Console.WriteLine(templ == Temp.Value2);
    }

    private enum Temp
    {
        Value1 = 1,
        Value2 = 1
    }
}

[Flags]
internal enum Week
{
    None = 0x0,
    Monday = 0x1,
    Tuesday = 0x2,
    Wednesday = 0x4,
    Thursday = 0x8,
    Friday = 0x10,
    Saturday = 0x20,
    Sunday = 0x40
}

internal class MyClass
{
    private Week _week = Week.Thursday | Week.Sunday;
}

internal class Program
{
    private static Week week;

    private static void Main(string[] args)
    {
        Test.NewMethod2();
    }

    //static void Main(string[] args)
    //{
    //	Person xc = new Person("小菜");

    //	Console.WriteLine("\n第一种装扮:");
    //	Sneakers pqx = new Sneakers();
    //	BigTrouser kk = new BigTrouser();
    //	TShirts dtx = new TShirts();

    //	// 装饰过程
    //	pqx.Decorate(xc);
    //	kk.Decorate(pqx);
    //	dtx.Decorate(kk);
    //	dtx.Show();

    //	Console.WriteLine("\n第二种装扮:");

    //	Leathershoes px = new Leathershoes();
    //	Tie ld = new Tie();
    //	Suit xz = new Suit();

    //	// 装饰过程
    //	px.Decorate(xc);
    //	ld.Decorate(px);
    //	xz.Decorate(ld);
    //	xz.Show();

    //	Console.WriteLine("\n第三种装扮:");
    //	Sneakers pqx2 = new Sneakers();
    //	LeatherShoes px2 = new Leathershoes();
    //	BigTrouser kk2 = new BigTrouser();
    //	Tie ld2 = new Tie();

    //	pqx2.Decorate(xc);
    //	px2.Decorate(pqx);
    //	kk2.Decorate(px2);
    //	ld2.Decorate(kk2);

    //	ld2.show();


    //	Console.Read();
    //}
}