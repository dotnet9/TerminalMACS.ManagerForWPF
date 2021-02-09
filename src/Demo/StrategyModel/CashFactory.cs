using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyModel
{
	/// <summary>
	/// 现金收费工厂类
	/// </summary>
	class CashFactory
	{
		public static CashSuper CreateCashAccept(string type)
		{
			CashSuper cs = null;

			switch(type)
			{
				case "正常收费":
					cs = new CashNormal();
					break;
				case "满300返100":
					CashReturn cr1 = new CashReturn("300", "100");
					cs = cr1;
					break;
				case "打八折":
					CashRebate cr2 = new CashRebate("0.8");
					cs = cr2;
					break;
			}

			return cs;
		}
	}
}
