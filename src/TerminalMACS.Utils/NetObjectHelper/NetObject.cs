using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerminalMACS.Utils.NetObjectHelper
{
    public static class NetObjectHelper
    {
        public static bool IsSame(this NetObject source, NetObjectAttribute dest)
        {
            return source.Name.Equals(dest.Name) && source.Version.Equals(dest.Version);
        }
    }

    public class NetObject
    {
        /// <summary>
        /// 获取或者设置 标识，唯一标识通信
        /// </summary>
        public int Mark { get; set; }

        /// <summary>
        /// 获取或者设置 对象名称，其实改为int更节约空间一点
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或者设置 对象版本，用于多版本支持
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 获取或者设置 任务ID，区分单次命令，响应时需要使用
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// 获取或者设置 对象序列化后的字节数组
        /// </summary>
        public byte[] Datas { get; set; }

        public override string ToString()
        {
            return $"Mark: {Mark},Name: {Name},Version: {Version},TaskID: {TaskID},  ByteLength: {Datas?.Length}";
        }
    }
}
