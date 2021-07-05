using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager.Sample
{
	public class TestMessage : Message
	{
		public TestMessage(object sender, string msg) : base(sender)
		{
			this.Msg = msg;
		}
		public string Msg { get; set; }
	}
}
