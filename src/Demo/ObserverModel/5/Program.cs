using System;

namespace ObserverModel._5
{
	delegate void EventHandler();
	class Program
	{
		static void Main(string[] args)
		{
			//老板胡汉三
			Boss huhansan = new Boss();

			//看股票的同事
			StockObserver tongshi1 = new StockObserver("魏关姹", huhansan);

			//看NBA的同事
			NBAObserver tongshi2 = new NBAObserver("易管查", huhansan);

			// 将“看股票者”的“关闭股票程序”方法和“看NBA者”的“关闭NBA直播”方法挂钩到“老板”的“更新”上，也就是将两不同类的不同方法委托给“老板”类的“更新”了
			huhansan.Update += new EventHandler(tongshi1.CloseStockMarket);
			huhansan.Update += new EventHandler(tongshi2.CloseNBADirectSeeding);

			//老板回来
			huhansan.SubjectState = "我胡汉三回来了! ";

			//发出通知			
			huhansan.Notify();

			Console.Read();
		}
	}
}
