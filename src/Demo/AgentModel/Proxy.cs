using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentModel
{
	/// <summary>
	/// 代理
	/// </summary>
	class Proxy:GiveGift
	{
		Pursuit gg;
		public Proxy(SchoolGirl mm)
		{
			gg = new Pursuit(mm);

		}

		public void GiveDolls()
		{
			gg.GiveDolls();
		}

		public void GiveFlowers()
		{
			gg.GiveFlowers();
		}

		public void GiveChocolate()
		{
			gg.GiveChocolate();
		}
	}
}
