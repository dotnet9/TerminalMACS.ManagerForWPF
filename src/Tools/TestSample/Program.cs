using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TestSample;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("通过多种方式比较两个列表长度、所包含元素是否相等，不考虑顺序");
        var lst1 = new List<string> { "2", "3", "1", "3" };
        var lst2 = new List<string> { "1", "3", "2" };
        Console.WriteLine($"lst1：({JsonConvert.SerializeObject(lst1)})");
        Console.WriteLine($"lst2：({JsonConvert.SerializeObject(lst2)})\r\n");

        CheckListCompareByLinq(); // linq方法比较
        CheckListCompareByLambda(); // 使用Lambda表达式比较
        CheckListCompareyByString(); // 排序转字符串后比较
        Console.ReadKey();
    }

    /// <summary>
    ///     1 使用Linq比较两个列表是否相等
    /// </summary>
    private static void CheckListCompareByLinq()
    {
        Console.WriteLine("第一种：使用Linq比较两个列表是否相等");
        var lst1 = new List<string> { "2", "3", "1", "3" };
        var lst2 = new List<string> { "3", "1", "3", "2" };
        var isEquals = lst1.SequenceEqual(lst2);

        Console.WriteLine($"两者直接通过(Enumerable.SequenceEqual(lst1, lst2))比较：{isEquals}");

        // 1、集合排序再比较
        lst1.Sort();
        lst2.Sort();
        isEquals = lst1.SequenceEqual(lst2);

        // 2、或者使用Linq的方式排序后再比较
        //isEquals = lst1.OrderBy(a => a).SequenceEqual(lst2.OrderBy(a=>a));

        Console.WriteLine($"两者排序后通过(Enumerable.SequenceEqual(lst1, lst2))比较：{isEquals}\r\n\r\n");
    }

    /// <summary>
    ///     2 使用lambda表达式比较两个列表是否相等
    /// </summary>
    private static void CheckListCompareByLambda()
    {
        Console.WriteLine("第二种：使用Lambda表达式比较两个列表是否相等");
        var lst1 = new List<string> { "2", "3", "1", "3" };
        var lst2 = new List<string> { "3", "1", "3", "2" };

        var q = from a in lst1
            join b in lst2 on a equals b
            select a;

        var isEquals = lst1.Count == lst2.Count
                       && q.Count() == lst1.Count;

        Console.WriteLine($"两者通过Lambda表达式比较：{isEquals}\r\n\r\n");
    }

    /// <summary>
    ///     3 列表排序转换为字符串再比较
    /// </summary>
    private static void CheckListCompareyByString()
    {
        Console.WriteLine("第三种：列表排序转换为字符串再比较");
        var lst1 = new List<string> { "2", "3", "1", "3" };
        var lst2 = new List<string> { "3", "1", "3", "2" };

        lst1.Sort();
        lst2.Sort();
        var lstStr1 = string.Join(',', lst1);
        var lstStr2 = string.Join(',', lst2);
        var isEquals = lstStr1.Equals(lstStr2);

        Console.WriteLine($"两者排序后通过（string.Join）转换为字符串：{isEquals}\r\n\r\n");
    }
}