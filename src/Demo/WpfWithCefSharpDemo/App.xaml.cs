using System;
using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;

namespace WpfWithCefSharpDemo
{

    public partial class App : Application
    {
        public App()
        {
            var jsonObj=new Dictionary<string, object>();
            jsonObj["id"] = 1;
            jsonObj["name"] = "沙漠尽头的狼";
            var address = new Dictionary<string, object>();
            address["address"] = "四川";
            address["code"] = "60000";
            jsonObj["address"] = address;

            var jsonStr = JsonConvert.SerializeObject(jsonObj);
            Console.WriteLine(jsonStr);
        }
    }
}
