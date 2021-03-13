using System;
using System.Collections;
using System.Collections.Generic;

namespace AgentModel
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Salary> companySalary = new List<Salary>()
			{
				new Salary() { Name = "Mike", BaseSalary = 3000, Bonus = 1000 },
				new Salary() { Name = "Rose", BaseSalary = 2000, Bonus = 4000 },
				new Salary() { Name = "Jeffry", BaseSalary = 1000, Bonus = 6000 },
				new Salary() { Name = "steve", BaseSalary = 4000, Bonus = 3000 }
			};
			//companySalary.Sort(new BonusComparer());    // 提供一个非默认的比较器
			companySalary.Sort((x, y) => x.Bonus.CompareTo(y.Bonus)); // 简写，Linq的写法很舒服

			foreach (Salary item in companySalary)
			{
				Console.WriteLine($"Name：{item.Name} \tBaseSalary: {item.BaseSalary.ToString()} \tBonus：{item.Bonus}");
			}

			Console.Read();
		}
	}


	class Salary : IComparable<Salary>
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

	class BonusComparer : IComparer<Salary>
	{
		#region Icomparer<Salary> 成员

		public int Compare(Salary x, Salary y)
		{
			return x.Bonus.CompareTo(y.Bonus);
		}

		#endregion
	}

}