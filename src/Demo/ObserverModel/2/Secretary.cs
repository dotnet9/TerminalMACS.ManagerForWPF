using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverModel._2
{
	/// <summary>
	/// 前台秘书类
	/// </summary>
	class Secretary
	{
		// 同事列表
		private IList<Observer> observers = new List<Observer>();
		private string action;


		// 增加
		// 针对抽象编程,减少了与具体类的耦合
		public void Attach(Observer observer)
		{
			observers.Add(observer);
		}


		// 减少
		// 针对抽象编程,减少了与具体类的耦合
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

		//前台状态
		public string SecretaryAction
		{
			get { return action; }
			set { action = value; }
		}
	}
}