using System;

namespace ObserverModel._5
{
	/// <summary>
	/// 看NBA的同事
	/// </summary>
	class NBAObserver 
	{
		private string name;
		private Subject sub;
		public NBAObserver(string name, Subject sub)
		{
			this.name = name;
			this.sub = sub;
		}

		// 方法“更新”名改为“关闭NBA直播”
		// 关闭NBA直播
		public void CloseNBADirectSeeding()
		{
			Console.WriteLine($"{sub.SubjectState}{name}关闭NBA直播，继续工作! ");

		}
	}
}
