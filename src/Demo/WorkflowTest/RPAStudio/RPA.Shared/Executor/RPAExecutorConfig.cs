using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Executor
{
    public class RPAExecutorStartupConfig
    {
        /// <summary>
        /// 是否处在调试状态
        /// </summary>
        public bool IsInDebuggingState { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Main.XAML路径
        /// </summary>
        public string MainXamlPath { get; set; }

        /// <summary>
        /// 管道名
        /// </summary>
        public string PipeName { get; set; }

        /// <summary>
        /// 需要加载的程序集路径
        /// </summary>
        public List<string> LoadAssemblyFromList { get; set; }

        /// <summary>
        /// 程序集解析参考的DLL路径
        /// </summary>
        public List<string> AssemblyResolveDllList { get; set; }


        /// <summary>
        /// 项目路径
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// 调试初始化JSON格式值
        /// </summary>
        public JObject InitialDebugJsonConfig { get; set; } = new JObject();

        /// <summary>
        /// 命令行调用时初始化JSON格式值
        /// </summary>
        public JObject InitialCommandLineJsonConfig { get; set; } = new JObject();


        /// <summary>
        /// 额外参数扩展
        /// </summary>
        public JObject JsonParams { get; set; } = new JObject();

    }
}
