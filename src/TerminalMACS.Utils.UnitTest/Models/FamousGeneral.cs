using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalMACS.Utils.NetObjectHelper;

namespace TerminalMACS.Utils.UnitTest.Models
{
    /// <summary>
    /// 三国名将
    /// </summary>
    public class FamousGeneral
    {
        /// <summary>
        /// 获取或者设置 编号
        /// </summary>
        [NetObjectProperty(ID = 1)]
        public int ID { get; set; }

        /// <summary>
        /// 获取或者设置 名字
        /// </summary>
        [NetObjectProperty(ID = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或者设置 描述
        /// </summary>
        [NetObjectProperty(ID = 3)]
        public string Memo { get; set; }

        public override string ToString()
        {
            return $"{ID}：{Name}=>{Memo}";
        }
    }
}
