using System.Collections.Generic;

namespace ObserverModel._3
{
	/// <summary>
	/// 前台秘书类
	/// </summary>
	class Secretary : Subject
	{
		// 同事列表
		private IList<Observer> observers = new List<Observer>();
		private string action;

		//增加
		public void Attach(Observer observer)
		{
			observers.Add(observer);
		}

		//减少
		public void Detach(Observer observer)
		{
			observers.Remove(observer);
		}

		//通知
		public void Notify()
		{
			foreach (Observer o in observers)
			{
				o.Update();
			}
		}

		//老板状态
		public string SubjectState
		{
			get { return action; }
			set { action = value; }
		}
	}
}