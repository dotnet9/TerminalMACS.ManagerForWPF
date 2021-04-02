using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeMode
{
	/// <summary>
	/// 工作经历
	/// 让“工作经历”实现ICloneable接口
	/// </summary>
	class WorkExperience :ICloneable
	{
		public string WorkDate { get; set; }

		public string Company { get; set; }

		/// <summary>
		/// "工作经历“类实现克隆方法
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return (Object)this.MemberwiseClone();
		}
	}
}
