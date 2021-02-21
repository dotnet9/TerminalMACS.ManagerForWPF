using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyModel
{
	// 抽象算法类
	abstract class Strategy
	{
		// 算法方法
		public abstract void AlgorithmInterface();
	}

	// 具体算法A
	class ConcreteStrategyA:Strategy
	{
		// 算法A实现方法
		public override void AlgorithmInterface()
		{
			Console.WriteLine("算法A实现");
		}
	}

	// 具体算法B
	class ConcreteStrategyB : Strategy
	{
		// 算法B实现方法
		public override void AlgorithmInterface()
		{
			Console.WriteLine("算法B实现");
		}
	}

	// 具体算法C
	class ConcreteStrategyC : Strategy
	{
		// 算法C实现方法
		public override void AlgorithmInterface()
		{
			Console.WriteLine("算法C实现");
		}
	}
}
