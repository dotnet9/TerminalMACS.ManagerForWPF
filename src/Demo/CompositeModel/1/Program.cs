//using System;

//namespace CompositeModel._1
//{
//	class Program
//	{
//		static void Main(string[] args)
//		{
//			// 生成树根root，根上长出两叶LeafA和LeafB
//			Composite root = new Composite("root");
//			root.Add(new Leaf("Leaf A"));
//			root.Add(new Leaf("Leaf B"));

//			// 根上长出分枝Composite X，分枝上也有两叶 LeafXA和LeafXB
//			Composite comp = new Composite("Composite X");
//			comp.Add(new Leaf("Leaf XA"));
//			comp.Add(new Leaf("Leaf XB"));

//			root.Add(comp);

//			// 在Composite X上再长出分枝CompositeXY，分枝上也有两叶LeafXYA和LeafXYB
//			Composite comp2 = new Composite("Composite XY");
//			comp2.Add(new Leaf("Leaf XYA"));
//			comp2.Add(new Leaf("Leaf XYB"));

//			comp.Add(comp2);

//			root.Add(new Leaf("Leaf C"));

//			Leaf leaf = new Leaf("Leaf D");
//			root.Add(leaf);
//			root.Remove(leaf);

//			root.Display(1);

//			Console.Read();
//		}
//	}
//}

