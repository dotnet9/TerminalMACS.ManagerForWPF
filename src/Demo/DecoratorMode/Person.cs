using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorMode
{
	class Person
	{
		public Person()
		{ }

		private string name;
		public Person(string name)
		{
			this.name = name;
		}

		public virtual void Show()
		{
			Console.WriteLine($"装扮的{name}");
		}
	}

}
