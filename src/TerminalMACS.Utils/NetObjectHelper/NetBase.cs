using System.Text;

namespace TerminalMACS.Utils.NetObjectHelper
{
    /// <summary>
    /// 通用数据标识
    /// </summary>
    public class NetBase
    {
        /// <summary>
        /// 信息标识
        /// </summary>
        public const int DDS_MAGIC = 0x4A534604;
        /// <summary>
        /// 通用字符集编码
        /// </summary>
        public static Encoding CommonEncoding = Encoding.UTF8;
    }
}
