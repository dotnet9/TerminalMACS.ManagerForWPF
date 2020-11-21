using System;
using System.Collections.Generic;
using System.Text;

namespace TestSample.BuilderPattern
{
    // 建造者模式

    /// <summary>
    /// 客户类
    /// </summary>
    class Customer
    {
        //static void Main(string[] args)
        //{
        //    // 客户找到电脑城老板说要买电脑，这里要装两台电脑
        //    // 创建指挥者和构造者
        //    Director director = new Director();

        //    Builder b1 = new ConcreteBuilder1();
        //    Builder b2 = new ConcreteBuilder2();

        //    // 老板叫员工去组装第一台电脑
        //    director.Construct(b1);

        //    // 组装完，组装人员搬来组装好的电脑
        //    Computer computer1 = b1.GetComputer();
        //    computer1.Show();

        //    // 老板叫员工去组装第二台电脑
        //    director.Construct(b2);

        //    Computer computer2 = b2.GetComputer();
        //    computer2.Show();

        //    Console.Read();
        //}
    }

    /// <summary>
    /// 小王和小李难道会自愿地去组装嘛，谁不想休息的，这必须有一个人叫他们去组装才会去的
    /// 这个人当然就是老板了，也就是建造者模式中的指挥者
    /// 指挥创建过程类
    /// </summary>
    public class Director
    {
        // 组装电脑
        public void Construct(Builder builder)
        {
            builder.BuildPartCPU();

            builder.BuildPartMainBoard();
        }
    }

    /// <summary>
    /// 电脑类
    /// </summary>
    public class Computer
    {
        // 电脑组件集合
        private IList<string> parts = new List<string>();

        // 把单个组件添加到电脑组件集合中
        public void Add(string part)
        {
            parts.Add(part);
        }

        public void Show()
        {
            Console.WriteLine("电脑开始在组装.......");
            foreach (string part in parts)
            {
                Console.WriteLine("组件" + part + "已装好");
            }
            Console.WriteLine("电脑组装好了");
        }
    }

    /// <summary>
    /// 抽象建造者，这个场景下为 "组装人" ，这里也可以定义为接口
    /// </summary>
    public abstract class Builder
    {

        // 装CPU
        public abstract void BuildPartCPU();

        // 装主板
        public abstract void BuildPartMainBoard();


        // 当然还有装硬盘，电源等组件，这里省略
        // 获得组装好的电脑
        public abstract Computer GetComputer();

    }

    /// <summary>
    /// 具体创建者，具体的某个人为具体创建者，例如：装机小王啊
    /// </summary>
    public class ConcreteBuilder1 : Builder
    {
        Computer computer = new Computer();

        public override void BuildPartCPU()
        {
            computer.Add("CPU1");
        }

        public override void BuildPartMainBoard()
        {
            computer.Add("Main board1");
        }

        public override Computer GetComputer()
        {
            return computer;
        }
    }

    /// <summary>
    /// 具体创建者，具体的某个人为具体创建者，例如：装机小李啊
    /// 又装另一台电脑了
    /// </summary>
    public class ConcreteBuilder2 : Builder
    {
        Computer computer = new Computer();

        public override void BuildPartCPU()
        {
            computer.Add("CPU2");
        }

        public override void BuildPartMainBoard()
        {
            computer.Add("Main board2");
        }

        public override Computer GetComputer()
        {
            return computer;
        }
    }
}
