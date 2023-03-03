using Microsoft.ClearScript.V8;
using System;

namespace CshaprWithJavascriptDemo {
    internal class Program
    {
        static void Main(string[] args)
        {
            using var engine = new V8ScriptEngine();
            //engine.DocumentSettings.AccessFlags = Microsoft.ClearScript.DocumentAccessFlags.EnableFileLoading;
            //engine.DefaultAccess = Microsoft.ClearScript.ScriptAccess.Full; // 这两行是为了允许加载js文件
            // do something
            var jsFunction = @"function say(name){
                            return 'Hello '+name;    
                        }";
            engine.Execute(jsFunction);
            var result = engine.Script.say("沙漠尽头的狼");
            Console.WriteLine(result);
        }
    }
}