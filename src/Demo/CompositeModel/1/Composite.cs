using System;
using System.Collections.Generic;

namespace CompositeModel._1
{
	class Composite : Component
	{
		// 一个子对象集合用来存储其下属的枝节点和叶节点
		private List<Component> children = new List<Component>();

		public Composite(string name) : base(name)
		{
		}

		public override void Add(Component c)
		{
			children.Add(c);
		}

		public override void Remove(Component c)
		{
			children.Remove(c);
		}

		public override void Display(int depth)
		{
			Console.WriteLine(new string('-', depth) + name);

			foreach (var item in children)
			{
				item.Display(depth + 2);
			}
		}
	}
}
