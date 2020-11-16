using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyModel
{
    public class UtilHelper
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimeStamp()
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1);    // 当地时区
            return (long)(DateTime.UtcNow - startTime).TotalMilliseconds;   // 相差毫秒数
        }
    }
}
