using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Share
{
    public class LogToOutputWindowTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        /// <summary>
        /// 输出文本并换行
        /// </summary>
        /// <param name="value">内容</param>
        public override void WriteLine(string value)
        {
            try
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Trace, value);
            }
            catch (Exception e)
            {
                //异常输出
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, "有一个错误产生", e.Message);
            }
        }
    }
}

