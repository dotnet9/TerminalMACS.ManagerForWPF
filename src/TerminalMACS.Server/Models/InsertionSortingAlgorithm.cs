using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TerminalMACS.Server.Models
{
	/// <summary>
	/// 插入排序
	/// </summary>
	class InsertionSortingAlgorithm : ISortingAlgorithm
	{
		public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
		{
			for (int i = 1; i < data.Count; i++)
			{
				var t = data[i];
				int j = i;
				while ((j > 0) && (data[j - 1].Value > t.Value))
				{
					data[j] = data[j - 1];      // 交换顺序

					updateData(data);
					--j;
				}
				data[j] = t;

				updateData(data);
			}
		}
	}
}
