using System;

namespace AdapterMode._2;

internal class Program
{
    private static void Main(string[] args)
    {
        Player b = new Forwards("巴蒂尔");
        b.Attack();

        Player m = new Guards("麦克格雷迪");
        m.Attack();

        // 翻译者告诉姚明，教练要求你既要‘进攻’又要‘防守’
        Player ym = new Translator("姚明");
        ym.Attack();
        ym.Defense();

        Console.Read();
    }
}