using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeMode
{
	abstract class Prototype
	{
		private string id;

		public Prototype(string id)
		{
			this.id = id;
		}

		public string Id
		{
			get { return id; }
		}

		/// <summary>
		/// 抽象类关键就是这样一个Clone方法
		/// </summary>
		/// <returns></returns>
		public abstract Prototype Clone();
	}

}
