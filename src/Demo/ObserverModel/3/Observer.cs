namespace ObserverModel._3
{
	/// <summary>
	/// 抽象的观察者
	/// </summary>
	abstract class Observer
	{
		protected string name;
		protected Subject sub;

		/// <summary>
		/// 通知
		/// </summary>
		/// <param name="name"></param>
		/// <param name="sub">原来是'前台'，现改成抽象通知者</param>

		public Observer(string name, Subject sub)
		{
			this.name = name;
			this.sub = sub;
		}

		public abstract void Update();
	}
}
