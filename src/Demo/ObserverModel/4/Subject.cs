using System.Collections.Generic;

namespace ObserverModel._4
{
	abstract class Subject
	{
		private IList<Observer> observers = new List<Observer>();

		//增加观察者
		public void Attach(Observer observer)
		{
			observers.Add(observer);
		}

		//移除观察者
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
	}
}
