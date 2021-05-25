using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
	/// <summary>
	/// 插入排序（Insertion Sort）
	/// </summary>
	class InsertionSortDemo
	{
		//static void Main(string[] args)
		//{
		//	int[] arr = { 3, 5, 8, 1, 0 };
		//	Console.WriteLine(string.Join(',', InsertionSort(arr)));
		//}

		public static int[] InsertionSort(int[] arr)
		{
			int len = arr.Length;

			int preIndex, current;
			for (int i = 1; i < len; i++)
			{
				preIndex = i - 1;
				current = arr[i];

				while (preIndex >= 0 && arr[preIndex] > current)
				{
					arr[preIndex + 1] = arr[preIndex];
					preIndex--;
				}

				arr[preIndex + 1] = current;
			}
			return arr;
		}
	}
}
