namespace ObserverModel._5
{
	class Boss : Subject
	{
		// 声明一事件Update，类型为委托EventHandler.
		// 声明一“EventHandler（事件处理程序)”的委托事件，名称叫“Update（更新)”
		public event EventHandler Update;

		private string action;

		public void Notify()
		{
			//  在访问“通知”方法时，调用“更新”
			Update();
		}

		public string SubjectState
		{
			get { return action; }
			set { action = value; }
		}
	}
}