using System;
using System.Collections.Generic;

namespace CompositeModel._2
{
	/// <summary>
	/// 具体公司，树枝节点
	/// </summary>
	class ConcreteCompany : Company
	{
		private List<Company> children = new List<Company>();

		public ConcreteCompany(string name)
			: base(name)
		{
		}

		public override void Add(Company c)
		{
			children.Add(c);
		}

		public override void Remove(Company c)
		{
			children.Remove(c);
		}

		public override void Display(int depth)
		{
			Console.WriteLine(new string('-', depth) + name);

			foreach (var component in children)
			{
				component.Display(depth + 2);
			}
		}

		// 履行职责
		public override void LineOfDuty()
		{
			foreach (var component in children)
			{
				component.LineOfDuty();
			}
		}
	}
}
