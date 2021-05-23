using System;
using System.Collections.Generic;

namespace TerminalMACS.Server.Models
{
	/// <summary>
	/// 希尔排序
	/// </summary>
	class ShellSortingAlgorithm : ISortingAlgorithm
	{
		public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
		{
			int inc;
			for (inc = 1; inc <= data.Count / 9; inc = 3 * inc + 1) ;
			for (; inc > 0; inc /= 3)
			{
				for (int i = inc + 1; i <= data.Count; i += inc)
				{
					var t = data[i - 1];
					int j = i;
					while ((j > inc) && (data[j - inc - 1].Value > t.Value))
					{
						data[j - 1] = data[j - inc - 1];//交换数据    
						updateData(data);
						j -= inc;
					}
					data[j - 1] = t;
					updateData(data);
				}
			}
		}
	}
}
