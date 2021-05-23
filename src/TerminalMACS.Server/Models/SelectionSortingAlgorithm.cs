using System;
using System.Collections.Generic;
using System.Threading;

namespace TerminalMACS.Server.Models
{
	/// <summary>
	/// 选择排序算法
	/// </summary>
	class SelectionSortingAlgorithm : ISortingAlgorithm
	{
		/// <summary>
		/// 按升序的方式排序
		/// </summary>
		/// <param name="data"></param>
		/// <param name="updateData"></param>
		public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
		{
			int min = default(int);
			for (int i = 0; i < data.Count - 1; ++i)
			{
				min = i;
				for (int j = i + 1; j < data.Count; ++j)
				{
					if (data[j].Value < data[min].Value)
					{
						min = j;
					}
				}
				var t = data[min];
				data[min] = data[i];
				data[i] = t;

				updateData(data);
			}
		}
	}
}
