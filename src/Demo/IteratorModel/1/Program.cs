using System;

namespace IteratorModel._1;

internal class Program
{
    private static void Main(string[] args)
    {
        // 公交车，即聚焦对象
        var a = new ConcreteAggregate();

        // 新上来的乘客，即对象数组
        a[0] = "大鸟";
        a[1] = "小菜";
        a[2] = "行李";
        a[3] = "老外";
        a[4] = "公交内部员工";
        a[5] = "小偷";

        // 售票员出场，先看好了上车的是哪些人，即声明了迭代器对象
        Iterator i = new ConcreteIterator(a);

        // 从第一个乘客开始
        var item = i.First();
        while (!i.IsDone())
        {
            // 对面前的乘客告知请买票
            Console.WriteLine($"{i.CurrentItem()} 请买票");

            // 下一乘客
            i.Next();
        }

        Console.Read();
    }
}