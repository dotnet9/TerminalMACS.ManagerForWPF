using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
	/// <summary>
	/// 计数排序（Counting Sort）,参考：https://lovejy.blog.csdn.net/article/details/81610602
	/// </summary>
	class CountingSortDemo
	{
		//public static void Main(string[] args)
		//{
		//	int[] array = { 43, 69, 11, 72, 28, 21, 56, 80, 48, 94, 32, 8 };

		//	CountingSort(array);

		//	Console.WriteLine(string.Join(',', array));
		//}

		public static void CountingSort(int[] array)
		{
			if (array.Length == 0) return;
			int min = array[0];
			int max = min;
			foreach (int number in array)
			{
				if (number > max)
				{
					max = number;
				}
				else if (number < min)
				{
					min = number;
				}
			}
			int[] counting = new int[max - min + 1];
			for (int i = 0; i < array.Length; i++)
			{
				counting[array[i] - min] += 1;
			}
			int index = -1;
			for (int i = 0; i < counting.Length; i++)
			{
				for (int j = 0; j < counting[i]; j++)
				{
					index++;
					array[index] = i + min;
				}
			}
		}
	}
}
