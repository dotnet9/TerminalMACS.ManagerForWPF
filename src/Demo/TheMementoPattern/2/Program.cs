//using System;

//namespace TheMementoPattern._2
//{
//	class Program
//	{
//		static void Main(string[] args)
//		{
//			// Originator初始状态，状态属性为“On”
//			Originator o = new Originator();
//			o.State = "On ";
//			o.Show();


//			Caretaker c = new Caretaker();

//			// 保存状态时，由于有了很好的封装，可以隐藏Originator的实现细节
//			c.Memento = o.CreateMemento();

//			// Originator改变了状态属性为“Off”
//			o.State = "off";
//			o.Show();

//			// 恢复原初始状态
//			o.SetMemento(c.Memento);
//			o.Show();

//			Console.Read();
//		}
//	}
//}

