using System;
using System.Collections.Generic;

namespace AgentModel;

internal class Program
{
    private static void Main(string[] args)
    {
        var companySalary = new List<Salary>
        {
            new() { Name = "Mike", BaseSalary = 3000, Bonus = 1000 },
            new() { Name = "Rose", BaseSalary = 2000, Bonus = 4000 },
            new() { Name = "Jeffry", BaseSalary = 1000, Bonus = 6000 },
            new() { Name = "steve", BaseSalary = 4000, Bonus = 3000 }
        };
        //companySalary.Sort(new BonusComparer());    // 提供一个非默认的比较器
        companySalary.Sort((x, y) => x.Bonus.CompareTo(y.Bonus)); // 简写，Linq的写法很舒服

        foreach (var item in companySalary)
            Console.WriteLine($"Name：{item.Name} \tBaseSalary: {item.BaseSalary.ToString()} \tBonus：{item.Bonus}");

        Console.Read();
    }
}

internal class Salary : IComparable<Salary>
{
    public string Name { get; set; }
    public int BaseSalary { get; set; }
    public int Bonus { get; set; }

    #region IComparable<Salary> 成员

    public int CompareTo(Salary other)
    {
        return BaseSalary.CompareTo(other.BaseSalary);
    }

    #endregion
}

internal class BonusComparer : IComparer<Salary>
{
    #region Icomparer<Salary> 成员

    public int Compare(Salary x, Salary y)
    {
        return x.Bonus.CompareTo(y.Bonus);
    }

    #endregion
}