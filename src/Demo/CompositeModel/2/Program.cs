using System;

namespace CompositeModel._2;

internal class Program
{
    private static void Main(string[] args)
    {
        var root = new ConcreteCompany("北京总公司");
        root.Add(new HRDepartment("总公司人力资源部"));
        root.Add(new FinanceDepartment("总公司财务部"));

        var comp = new ConcreteCompany("上海华东分公司");
        comp.Add(new HRDepartment("华东分公司人力资源部"));
        comp.Add(new FinanceDepartment("华东分公司财务部"));

        root.Add(comp);

        var comp1 = new ConcreteCompany("南京办事处");
        comp.Add(new HRDepartment("南京办事处人力资源部"));
        comp.Add(new FinanceDepartment("南京办事处财务部"));

        root.Add(comp1);

        var comp2 = new ConcreteCompany("杭州办事处");
        comp.Add(new HRDepartment("杭州办事处人力资源部"));
        comp.Add(new FinanceDepartment("杭州办事处财务部"));

        root.Add(comp2);

        Console.WriteLine("\n结构图：");
        root.Display(1);

        Console.WriteLine("\n职责：");
        root.LineOfDuty();

        Console.Read();
    }
}