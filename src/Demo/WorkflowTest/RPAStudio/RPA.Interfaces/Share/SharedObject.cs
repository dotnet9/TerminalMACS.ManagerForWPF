using RPA.Interfaces.AppDomains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Share
{
    public class SharedObject : MarshalByRefServiceBase
    {
        /// <summary>
        /// 日志输出类型
        /// </summary>
        public enum enOutputType
        {
            Trace,
            Information,
            Warning,
            Error,
        }


        /// <summary>
        /// 当前项目所在路径
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// 当前程序EXE所在的路径
        /// </summary>
        public string ApplicationCurrentDirectory
        {
            get
            {
                var exeFullPath = this.GetType().Assembly.Location;
                var exeDir = System.IO.Path.GetDirectoryName(exeFullPath);
                return exeDir;
            }
        }

        /// <summary>
        /// 输出代理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="msg">消息</param>
        /// <param name="msgDetails">消息详情</param>
        public delegate void OutputDelegate(enOutputType type, string msg, string msgDetails = "");

        public event OutputDelegate OutputEvent;


        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="msg">消息</param>
        /// <param name="msgDetails">消息详情</param>
        public void Output(enOutputType type, object msg, object msgDetails = null)
        {
            var msgStr = msg == null ? "" : msg.ToString();
            var msgDetailsStr = msgDetails == null ? msgStr : msgDetails.ToString();
            OutputEvent?.Invoke(type, msgStr, msgDetailsStr);
        }

        /// <summary>
        /// 是否高亮元素(调试选项中设置)
        /// </summary>
        public bool isHighlightElements { get; set; }

        private static SharedObject _instance = null;
        public static SharedObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SharedObject();
                }

                return _instance;
            }

            private set
            {
                _instance = value;
            }
        }


        public static void SetCrossDomainInstance(SharedObject instance)
        {
            Instance = instance;
        }

    }
}
