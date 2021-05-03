using System;

namespace CompositeModel._2
{
	// 人力资源部与财务部，树叶节点
	class HRDepartment : Company
	{
		public HRDepartment(string name) : base(name)
		{
		}

		public override void Add(Company c)
		{
		}

		public override void Remove(Company c)
		{
		}

		public override void Display(int depth)
		{
			Console.WriteLine(new string('-', depth) + name);
		}

		public override void LineOfDuty()
		{
			Console.WriteLine($"{name}员工招聘培训管理");
		}
	}
}
