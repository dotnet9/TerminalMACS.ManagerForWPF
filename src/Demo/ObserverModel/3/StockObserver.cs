using System;

namespace ObserverModel._3
{
	/// <summary>
	/// 看股票的同事
	/// </summary>
	class StockObserver : Observer
	{
		/// <summary>
		/// 通知
		/// </summary>
		/// <param name="name"></param>
		/// <param name="sub">原来是'前台'，现改成抽象通知者</param>
		public StockObserver(string name, Subject sub) : base(name, sub)
		{

		}

		public override void Update()
		{
			Console.WriteLine($"{sub.SubjectState} {name}关闭股票行情，继续工作! ");
		}
	}

}
