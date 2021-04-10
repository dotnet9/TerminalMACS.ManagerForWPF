using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverModel._4
{
	class ConcreteObserver : Observer
	{
		private string name;
		private string observerState;
		private ConcreteSubject subject;

		public ConcreteObserver(ConcreteSubject subject, string name)
		{
			this.subject = subject;
			this.name = name;
		}

		public override void Update()
		{
			observerState = subject.SubjectState;
			Console.WriteLine($"观察者{name}的新状态是{observerState}");
		}

		public ConcreteSubject Subject
		{
			get { return subject; }
			set { subject = value; }
		}
	}
}