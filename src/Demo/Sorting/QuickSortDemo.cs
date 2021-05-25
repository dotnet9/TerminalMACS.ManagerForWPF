using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
	/// <summary>
	/// 快速排序（Quick Sort）
	/// </summary>
	class QuickSortDemo
	{
		//public static void Main(string[] args)
		//{
		//	int[] arr = { 51, 46, 20, 18, 65, 97, 82, 30, 77, 50 };
		//	Console.WriteLine(string.Join(',', QuickSort(arr, 0, arr.Length - 1)));

		//}

		// 分治法
		public static int[] QuickSort(int[] arr, int low, int high)
		{
			if (low < high)
			{
				int pivot = Partition(arr, low, high); // 将数组分为两部分，pivot枢轴
				QuickSort(arr, low, pivot - 1);
				QuickSort(arr, pivot + 1, high);
			}
			return arr;
		}

		private static int Partition(int[] arr, int low, int high)
		{
			int pivot = arr[low];
			while (low < high)
			{
				while (low < high && arr[high] >= pivot)
					--high;
				arr[low] = arr[high];
				while (low < high && arr[low] <= pivot)
					++low;
				arr[high] = arr[low];
			}
			arr[low] = pivot;

			return low;
		}
	}
}
