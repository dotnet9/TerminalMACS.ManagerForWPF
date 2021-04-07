using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
	class Product
	{
		IList<string> parts = new List<string>();

		// 添加产品部件
		public void Add(string part)
		{
			parts.Add(part);
		}

		public void Show()
		{
			Console.WriteLine("\n产品创建----");

			// 列举所有的产品部件
			foreach (string part in parts)
			{
				Console.WriteLine(part);
			}
		}
	}
}
