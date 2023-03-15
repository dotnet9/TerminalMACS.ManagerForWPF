using Microsoft.ClearScript.V8;
using System;

namespace CshaprWithJavascriptDemo
{
    /// <summary>
    /// ClearScript库使用示例
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            ExecuteJsCode();
            InteractionBetweenJsAndCsharp();
            JsCallCSharpMethod();
            Console.ReadLine();
        }

        /// <summary>
        /// 执行JS代码
        /// </summary>
        static void ExecuteJsCode()
        {
            using var engine = new V8ScriptEngine();
            engine.Execute("var a = 10; var b = 20; var c = a + b;");
            var result = engine.Script.c;
            Console.WriteLine(result); // 输出 30
        }

        /// <summary>
        /// JS与C#交互
        /// </summary>
        static void InteractionBetweenJsAndCsharp()
        {
            using var engine = new V8ScriptEngine();
            var person = new Person { Name = "沙漠尽头的狼", Age = 18 };
            engine.AddHostObject("person", person);
            engine.Execute("var c = person.Name + ' 才 ' + person.Age + ' 岁呀？';");
            var result = engine.Script.c;
            Console.WriteLine(result); // 沙漠尽头的狼 才 18 岁呀？
        }

        /// <summary>
        /// JS调用C#的方法
        /// </summary>
        static void JsCallCSharpMethod()
        {
            using var engine = new V8ScriptEngine();
            var calculator = new Calculator();
            engine.AddHostObject("calculator", calculator);
            engine.Execute("var result = calculator.Add(15, 20)");
            var result = engine.Script.result;
            Console.WriteLine(result); // 35
        }
    }

    /// <summary>
    /// Person类需要为Public,V8引擎才能正常访问
    /// </summary>
    public class Person
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}