using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorMode
{
	class Finery : Person
	{
		protected Person component;

		// 打扮
		public void Decorate(Person component)
		{
			this.component = component;
		}

		public override void Show()
		{
			if (component != null)
			{
				component.Show();
			}
		}
	}

	class TShirts : Finery
	{
		public override void Show()
		{
			Console.Write("大T恤");
			base.Show();
		}
	}

	class BigTrouser : Finery
	{
		public override void Show()
		{
			Console.Write("垮裤");
			base.Show();
		}
	}

}
