using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyModel
{

	class CashContext
	{
		CashSuper cs = null;

		// 注意参数不是具体的收费策略对象，而是一个字符串，表示收费类型
		// 将实例化具体策略的过程由客户端转移到Context类中。简单工厂的应用
		public CashContext(string type)
		{
			switch (type)
			{
				case "正常收费":
					cs = new CashNormal();
					break;
				case "满300返100":
					cs = new CashReturn("300", "100");
					break;
				case "打8折":
					cs = new CashRebate("0.8");
					break;
			}
		}

		public double GetResult(double money)
		{
			return cs.AcceptCash(money);
		}
	}
}
