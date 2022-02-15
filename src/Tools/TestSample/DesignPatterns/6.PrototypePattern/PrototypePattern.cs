namespace TestSample._6.PrototypePattern;
// 原型模式

///火影忍者中鸣人的影分身和孙悟空的的变都是原型模式
internal class Client
{
    /*static void Main(string[] args)
    {
        // 孙悟空 原型
        MonkeyKingPrototype prototypeMonkeyKing = new ConcretePrototype("MonkeyKing");

        // 变一个
        MonkeyKingPrototype cloneMonkeyKing = prototypeMonkeyKing.Clone() as ConcretePrototype;
        Console.WriteLine("Cloned1:\t" + cloneMonkeyKing.Id);

        // 变两个
        MonkeyKingPrototype cloneMonkeyKing2 = prototypeMonkeyKing.Clone() as ConcretePrototype;
        Console.WriteLine("Cloned2:\t" + cloneMonkeyKing2.Id);

        Console.ReadLine();
    }*/
}

/// <summary>
///     孙悟空原型
/// </summary>
public abstract class MonkeyKingPrototype
{
    public MonkeyKingPrototype(string id)
    {
        Id = id;
    }

    public string Id { get; set; }

    // 克隆方法，即孙大圣说“变”
    public abstract MonkeyKingPrototype Clone();
}

/// <summary>
///     创建具体原型
/// </summary>
public class ConcretePrototype : MonkeyKingPrototype
{
    public ConcretePrototype(string id)
        : base(id)
    {
    }

    /// <summary>
    ///     浅拷贝
    /// </summary>
    /// <returns></returns>
    public override MonkeyKingPrototype Clone()
    {
        // 调用MemberwiseClone方法实现的是浅拷贝，另外还有深拷贝
        return (MonkeyKingPrototype)MemberwiseClone();
    }
}