using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyModel
{
	class Program
	{
		static void Main(string[] args)
		{
			CashContext context;

			// 由于实例化不同的策略，所以最终在调用context.ContextInterface();时，所获得的结果就不尽相同

			context = new CashContext(new ConcreteStrategyA());
			context.ContextInterface();

			context = new CashContext(new ConcreteStrategyB());
			context.ContextInterface();

			context = new CashContext(new ConcreteStrategyC());
			context.ContextInterface();
		}
	}
}
