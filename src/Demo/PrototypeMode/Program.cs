using System;

namespace PrototypeMode
{
	class Program
	{
		static void Main(string[] args)
		{
			Resume a = new Resume("大鸟");
			a.SetPersonalInfo("男", "29");
			a.SetWorkExperience("1998 - 2000", "XX 公司");

			// b 和c都克隆于a，但当它们都设置了“工作经历”时,我们希望的结果是三个的显示不一样
			Resume b = (Resume)a.Clone();
			b.SetWorkExperience("1998-2006", "YY 企业");

			Resume c = (Resume)a.Clone();
			b.SetWorkExperience("1998-2003", "zz 企业");

			a.Display();
			b.Display();
			c.Display();

			Console.Read();
		}
	}
}
