using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
	class ConcreteBuilder1 : Builder
	{
		private Product product = new Product();

		// 建造具体的两个部件是部件A和部件B
		public override void BuildPartA()
		{

			product.Add("部件A");
		}

		public override void BuildPartB()
		{
			product.Add("部件B");
		}

		public override Product GetResult()
		{
			return product;
		}
	}
}
