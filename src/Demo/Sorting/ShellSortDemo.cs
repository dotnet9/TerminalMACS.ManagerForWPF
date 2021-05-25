using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
	/// <summary>
	/// 希尔排序（Shell Sort）
	/// </summary>
	class ShellSortDemo
	{
		//public static void Main(string[] args)
		//{
		//	int[] arr = { 3, 5, 8, 1, 0 };
		//	Console.WriteLine(string.Join(',', ShellSort(arr)));
		//}

		public static int[] ShellSort(int[] data)
		{
			int j = 0;
			int temp = 0;
			for (int increment = data.Length / 2; increment > 0; increment /= 2)
			{
				for (int i = increment; i < data.Length; i++)
				{
					temp = data[i];
					for (j = i - increment; j >= 0; j -= increment)
					{
						if (temp < data[j])
						{
							data[j + increment] = data[j];
						}
						else
						{
							break;
						}
					}
					data[j + increment] = temp;
				}
			}
			return data;
		}
	}
}
