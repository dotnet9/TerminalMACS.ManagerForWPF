using System;
using System.Collections.Generic;
using System.Text;

namespace TestSample.Factory.NoSimpleFactory
{
    /// <summary>
    /// 自己做饭的情况
    /// 没有简单工厂之前，客户想吃什么菜只能自己炒的
    /// </summary>
    public class NoSimpleFactory
    {
        /// <summary>
        /// 烧菜方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Food Cook(string type)
        {
            Food food = null;
            // 客户A说：我想吃西红柿炒蛋怎么办？
            // 客户B说：那你就自己烧啊
            // 客户A说： 好吧，那就自己做吧
            if (type.Equals("西红柿炒蛋"))
            {
                food = new TomatoScrambledEggs();
            }
            // 我又想吃土豆肉丝, 这个还是得自己做
            // 我觉得自己做好累哦，如果能有人帮我做就好了？
            else if (type.Equals("土豆肉丝"))
            {
                food = new ShreddedPorkWithPotatoes();
            }
            return food;
        }

       /* static void Main(string[] args)
        {
            // 做西红柿炒蛋
            Food food1 = Cook("西红柿炒蛋");
            food1.Print();

            Food food2 = Cook("土豆肉丝");
            food2.Print();

            Console.Read();
        }*/
    }
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
            Console.WriteLine("一份西红柿炒蛋！");
        }
    }

    /// <summary>
    /// 土豆肉丝这道菜
    /// </summary>
    public class ShreddedPorkWithPotatoes : Food
    {
        public override void Print()
        {
            Console.WriteLine("一份土豆肉丝");
        }
    }
}
