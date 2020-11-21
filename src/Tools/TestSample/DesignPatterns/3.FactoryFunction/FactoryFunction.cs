using System;
using System.Collections.Generic;
using System.Text;

namespace TestSample.Factory.FactoryFunction
{
    // 工厂方法

    /// <summary>
    /// 菜抽象类
    /// </summary>
    public abstract class Food
    {
        // 输出点了什么菜
        public abstract void Print();
    }

    /// <summary>
    /// 西红柿炒鸡蛋这道菜
    /// </summary>
    public class TomatoScrambledEggs : Food
    {
        public override void Print()
        {
            Console.WriteLine("西红柿炒蛋好了！");
        }
    }

    /// <summary>
    /// 土豆肉丝这道菜
    /// </summary>
    public class ShreddedPorkWithPotatoes : Food
    {
        public override void Print()
        {
            Console.WriteLine("土豆肉丝好了");
        }
    }

    /// <summary>
    /// 抽象工厂类
    /// </summary>
    public abstract class Creator
    {
        // 工厂方法
        public abstract Food CreateFoddFactory();
    }

    /// <summary>
    /// 西红柿炒蛋工厂类
    /// </summary>
    public class TomatoScrambledEggsFactory : Creator
    {
        /// <summary>
        /// 负责创建西红柿炒蛋这道菜
        /// </summary>
        /// <returns></returns>
        public override Food CreateFoddFactory()
        {
            return new TomatoScrambledEggs();
        }
    }

    /// <summary>
    /// 土豆肉丝工厂类
    /// </summary>
    public class ShreddedPorkWithPotatoesFactory : Creator
    {
        /// <summary>
        /// 负责创建土豆肉丝这道菜
        /// </summary>
        /// <returns></returns>
        public override Food CreateFoddFactory()
        {
            return new ShreddedPorkWithPotatoes();
        }
    }

    /// <summary>
    /// 客户端调用
    /// </summary>
    class Client
    {
        //static void Main(string[] args)
        //{
        //    // 初始化做菜的两个工厂（）
        //    Creator shreddedPorkWithPotatoesFactory = new ShreddedPorkWithPotatoesFactory();
        //    Creator tomatoScrambledEggsFactory = new TomatoScrambledEggsFactory();

        //    // 开始做西红柿炒蛋
        //    Food tomatoScrambleEggs = tomatoScrambledEggsFactory.CreateFoddFactory();
        //    tomatoScrambleEggs.Print();

        //    //开始做土豆肉丝
        //    Food shreddedPorkWithPotatoes = shreddedPorkWithPotatoesFactory.CreateFoddFactory();
        //    shreddedPorkWithPotatoes.Print();

        //    Console.Read();
        //}
    }
}