using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverModel._2
{
	/// <summary>
	/// 看NBA的同事
	/// </summary>
	class NBAObserver : Observer
	{
		public NBAObserver(string name, Secretary sub) : base(name, sub)
		{

		}

		public override void Update()
		{
			Console.WriteLine($"{sub.SecretaryAction} {name}关闭NBA直播，继续工作! ");
		}
	}
}
