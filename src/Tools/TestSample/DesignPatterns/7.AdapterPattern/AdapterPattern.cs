using System;

/// 这里以插座和插头的例子来诠释适配器模式
/// 现在我们买的电器插头是2个孔，但是我们买的插座只有3个孔的
/// 这是我们想把电器插在插座上的话就需要一个电适配器
namespace TestSample._7.AdapterPattern;

/// <summary>
///     客户端，客户想要把2个孔的插头 转变成三个孔的插头，这个转变交给适配器就好
///     既然适配器需要完成这个功能，所以它必须同时具体2个孔插头和三个孔插头的特征
/// </summary>
internal class Client
{
    /*static void Main(string[] args)
    {
        // 现在客户端可以通过电适配要使用2个孔的插头了
        IThreeHole threehole = new PowerAdapter();
        threehole.Request();
        Console.ReadLine();
    }*/
}

/// <summary>
///     三个孔的插头，也就是适配器模式中的目标角色
/// </summary>
public interface IThreeHole
{
    void Request();
}

/// <summary>
///     两个孔的插头，源角色——需要适配的类
/// </summary>
public abstract class TwoHole
{
    public void SpecificRequest()
    {
        Console.WriteLine("我是两个孔的插头");
    }
}

/// <summary>
///     适配器类，接口要放在类的后面
///     适配器类提供了三个孔插头的行为，但其本质是调用两个孔插头的方法
/// </summary>
public class PowerAdapter : TwoHole, IThreeHole
{
    /// <summary>
    ///     实现三个孔插头接口方法
    /// </summary>
    public void Request()
    {
        // 调用两个孔插头方法
        SpecificRequest();
    }
}