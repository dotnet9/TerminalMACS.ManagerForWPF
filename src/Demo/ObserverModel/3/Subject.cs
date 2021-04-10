namespace ObserverModel._3
{
	// 通知者接口
	interface Subject
	{
		void Attach(Observer observer);
		void Detach(Observer observer);
		void Notify();
		string SubjectState
		{
			get; set;
		}
	}
}
