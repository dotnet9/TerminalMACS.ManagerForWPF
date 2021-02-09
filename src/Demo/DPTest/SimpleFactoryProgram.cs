using System;
using System.Security.Cryptography;

namespace DPTest
{
	class Program
	{
		//static void Main(string[] args)
		//{
		//	try
		//	{
		//		Console.Write("请输入数字A：");
		//		string strNumberA = Console.ReadLine();
		//		Console.WriteLine("请选择运算符号（+、-、*、/）：");
		//		string strOperate = Console.ReadLine();
		//		Console.Write("请输入数字B：");
		//		string strNumberB = Console.ReadLine();
		//		Operation oper;
		//		oper = OperationFactory.CreateOpearte("+");
		//		oper.NumberA = 1;
		//		oper.NumberB = 2;
		//		double result = oper.GetResult();
		//		Console.WriteLine("结果是：" + result);
		//		Console.ReadLine();
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine("您的输入有错：" + ex.Message);
		//	}
		//}
	}

	public class OperationFactory
	{
		public static Operation CreateOpearte(string operate)
		{
			Operation oper = null;
			switch(operate)
			{
				case "+":
					oper = new OperationAdd();
					break;
				case "-":
					oper = new OperationSub();
					break;
				case "*":
					oper = new OperationMul();
					break;
				case "/":
					oper = new OperationDIv();
					break;
			}

			return oper;
		}
	}

	public class Operation
	{
		public double NumberA { get; set; }
		public double NumberB { get; set; }
		public virtual double GetResult()
		{
			double result = 0;
			return result;
		}
	}

	/// <summary>
	/// 加法类，继承运算类
	/// </summary>
	public class OperationAdd : Operation
	{
		public override double GetResult()
		{
			double result = 0;
			result = NumberA + NumberB;
			return result;
		}
	}

	/// <summary>
	/// 减法类，继承运算类
	/// </summary>
	public class OperationSub : Operation
	{
		public override double GetResult()
		{
			double result = 0;
			result = NumberA - NumberB;
			return result;
		}
	}

	/// <summary>
	/// 乘法类，继承运算类
	/// </summary>
	public class OperationMul : Operation
	{
		public override double GetResult()
		{
			double result = 0;
			result = NumberA * NumberB;
			return result;
		}
	}

	/// <summary>
	/// 除法类，继承运算类
	/// </summary>
	public class OperationDIv : Operation
	{
		public override double GetResult()
		{
			double result = 0;
			if (NumberB == 0)
				throw new Exception("除数不能为0。");
			result = NumberA / NumberB;
			return result;
		}
	}
}
