using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeModel._2
{
	abstract class Company
	{
		protected string name;

		public Company(string name)
		{
			this.name = name;
		}

		public abstract void Add(Company c);        // 增加
		public abstract void Remove(Company c);     // 移除
		public abstract void Display(int depth);    // 显示
		public abstract void LineOfDuty();			// 发行职责（不同的部门需履行不同的职责）
	}
}
