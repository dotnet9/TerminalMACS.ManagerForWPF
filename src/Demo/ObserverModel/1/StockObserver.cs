using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverModel._1
{
	/// <summary>
	/// 看股票同事类
	/// </summary>
	class StockObserver
	{
		private string name;
		private Secretary sub;

		public StockObserver(string name, Secretary sub)
		{

			this.name = name;
			this.sub = sub;
		}

		public void Update()
		{
			Console.WriteLine($"{sub.SecretaryAction}{name}关闭股票行情，继续工作! ");
		}
	}
}