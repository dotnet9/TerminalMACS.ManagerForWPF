namespace ObserverModel._5
{
	/// <summary>
	/// 通知者接口
	/// </summary>
	interface Subject
	{
		void Notify();

		string SubjectState { get; set; }
	}
}
