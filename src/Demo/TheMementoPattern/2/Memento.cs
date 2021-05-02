namespace TheMementoPattern._2
{
	/// <summary>
	/// 备忘录类
	/// </summary>
	class Memento
	{
		public string State { get; private set; }

		/// <summary>
		/// 构造方法，将相关数据导入
		/// </summary>
		/// <param name="state"></param>
		public Memento(string state)
		{
			this.State = state;
		}
	}
}
