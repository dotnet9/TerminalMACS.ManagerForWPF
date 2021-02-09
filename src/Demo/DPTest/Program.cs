using System;
using System.Security.Cryptography;

namespace DPTest
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Console.Write("请输入数字A：");
				string strNumberA = Console.ReadLine();
				Console.WriteLine("请选择运算符号（+、-、*、/）：");
				string strOperate = Console.ReadLine();
				Console.Write("请输入数字B：");
				string strNumberB = Console.ReadLine();
				string strResult = "";

				switch(strOperateB)
				if ("+" == strOperate)
					strResult = Convert.ToString(Convert.ToDouble(strNumberA) + Convert.ToDouble(strNumberB));
				else if ("-" == strOperate)
					strResult = Convert.ToString(Convert.ToDouble(strNumberA) - Convert.ToDouble(strNumberB));
				else if ("*" == strOperate)
					strResult = Convert.ToString(Convert.ToDouble(strNumberA) * Convert.ToDouble(strNumberB));
				else if ("/" == strOperate)
					strResult = Convert.ToString(Convert.ToDouble(strNumberA) / Convert.ToDouble(strNumberB));

				Console.WriteLine("结果是：" + strResult);
			}
			catch (Exception ex)
			{

			}
		}
	}
}
