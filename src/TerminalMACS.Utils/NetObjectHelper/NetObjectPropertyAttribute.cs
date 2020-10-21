using System;

namespace TerminalMACS.Utils.NetObjectHelper
{
    /// <summary>
    /// 可序列化字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class NetObjectPropertyAttribute : Attribute
    {
        /// <summary>
        /// 获取或者设置序列化的顺序
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 获取或者设置字段类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 获取或者设置字段值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 获取或者设置当前对象引用
        /// </summary>
        public object Obj { get; set; }


        private System.Text.Encoding _CurrentEncoding = NetBase.CommonEncoding;
        /// <summary>
        /// 获取或者设置字符编码
        /// </summary>
        public System.Text.Encoding CurrentEncoding
        {
            get { return _CurrentEncoding; }
            set { _CurrentEncoding = value; }
        }


        public override string ToString()
        {
            return $"Type={Type},value={Value},obj={Obj}";
        }
    }
}
