namespace IteratorModel._1
{
	/// <summary>
	/// 迭代器抽象类，用于定义得到开始对象、得到下一个对象、判断是否到结尾、当前对象等抽象方法,统一接口
	/// </summary>
	abstract class Iterator
	{
		public abstract object First();
		public abstract object Next();
		public abstract bool IsDone();
		public abstract object CurrentItem();
	}
}
