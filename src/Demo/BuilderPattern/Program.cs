using System;

namespace BuilderPattern
{
	class Program
	{
		static void Main(string[] args)
		{
			Director director = new Director();
			Builder b1 = new ConcreteBuilder1();
			Builder b2 = new ConcreteBuilder2();
			director.Construct(b1);

			// 指挥者用ConcreteBuilder1的方法来建造产品
			Product p1 = b1.GetResult();
			p1.Show();
			director.Construct(b2);

			// 指挥者用ConcreteBuilder2的方法来建造产品
			Product p2 = b2.GetResult();
			p2.Show();

			Console.Read();
		}
	}
}
