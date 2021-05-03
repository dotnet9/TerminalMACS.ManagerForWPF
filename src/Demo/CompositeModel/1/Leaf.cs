using System;

namespace CompositeModel._1
{
	class Leaf : Component
	{
		public Leaf(string name) : base(name)
		{
		}

		// 由于叶子没有再增加分支和树叶，所以Add和Remove方法实现它没有意义，但这样做可以消除叶节点和枝节点对象在抽象层次的区别，它们具备安全一致的接口

		public override void Add(Component c)
		{
			Console.WriteLine("Cannot add to a leaf");
		}
		public override void Remove(Component c)
		{
			Console.WriteLine("Cannot remove from a leaf");
		}

		// 叶节点的具体方法，此处是显示其名称和级别
		public override void Display(int depth)
		{
			Console.WriteLine(new string('-', depth) + name);
		}

	}
}
