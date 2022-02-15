using System;

namespace StrategyModel;

// 抽象算法类
internal abstract class Strategy
{
    // 算法方法
    public abstract void AlgorithmInterface();
}

// 具体算法A
internal class ConcreteStrategyA : Strategy
{
    // 算法A实现方法
    public override void AlgorithmInterface()
    {
        Console.WriteLine("算法A实现");
    }
}

// 具体算法B
internal class ConcreteStrategyB : Strategy
{
    // 算法B实现方法
    public override void AlgorithmInterface()
    {
        Console.WriteLine("算法B实现");
    }
}

// 具体算法C
internal class ConcreteStrategyC : Strategy
{
    // 算法C实现方法
    public override void AlgorithmInterface()
    {
        Console.WriteLine("算法C实现");
    }
}