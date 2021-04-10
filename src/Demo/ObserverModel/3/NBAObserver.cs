using System;

namespace ObserverModel._3
{
	/// <summary>
	/// 看NBA的同事
	/// </summary>
	class NBAObserver : Observer
	{
		public NBAObserver(string name, Subject sub) : base(name, sub)
		{

		}

		public override void Update()
		{
			Console.WriteLine($"{sub.SubjectState} {name}关闭NBA直播，继续工作! ");
		}
	}
}
