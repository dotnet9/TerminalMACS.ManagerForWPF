using System;
using System.Diagnostics;

namespace ConsoleAppForDotnet6
{
	class Program
	{
		static void Main(string[] args)
		{
			Employee mike = new Employee()
			{
				IDCode = "NB123",
				Age = 30,
				Department = new Department
				{
					Name = "Dep1"
				}
			};

			Employee rose = mike.Clone() as Employee;

			Console.WriteLine(rose.IDCode);
			Console.WriteLine(rose.Age);
			Console.WriteLine(rose.Department);

			Console.WriteLine("开始改变mike的值: ");

			mike.IDCode = "NB456";
			mike.Age = 60;
			mike.Department.Name = "Dep2";

			Console.WriteLine(rose.IDCode);
			Console.WriteLine(rose.Age);
			Console.WriteLine(rose.Department);

		}
	}
}


