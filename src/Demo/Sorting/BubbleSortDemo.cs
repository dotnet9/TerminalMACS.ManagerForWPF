using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
	/// <summary>
	/// 冒泡排序（Bubble Sort）
	/// </summary>
	class BubbleSortDemo
	{
		//static void Main(string[] args)
		//{
		//	int[] arr = { 3, 5, 8, 1, 0 };
		//	Console.WriteLine(string.Join(',', BubbleSort(arr)));
		//}

		public static int[] BubbleSort(int[] arr)
		{
			int temp = 0;
			for (int i = 0; i < arr.Length - 1; i++)
			{
				for (int j = 0; j < arr.Length - 1 - i; j++)
				{
					if (arr[j] > arr[j + 1])
					{
						temp = arr[j + 1];
						arr[j + 1] = arr[j];
						arr[j] = temp;
					}
				}
			}
			return arr;
		}
	}
}
