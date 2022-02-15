namespace AgentModel;


//abstract class Subject
//{
//	public abstract void Request();
//}

//class Realsubject : Subject
//{
//	public override void Request()
//	{
//		Console.WriteLine("真实的请求");
//	}
//}

//class Proxy : Subject
//{
//	Realsubject realSubject;
//	public override void Request()
//	{
//		if (realSubject == null)
//		{
//			realSubject = new Realsubject();
//		}
//		realSubject.Request();
//	}
//}