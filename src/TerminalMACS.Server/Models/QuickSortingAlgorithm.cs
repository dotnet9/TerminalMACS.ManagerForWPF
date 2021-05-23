using System;
using System.Collections.Generic;
using System.Threading;

namespace TerminalMACS.Server.Models
{
	class QuickSortingAlgorithm : ISortingAlgorithm
	{
		public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
		{
			Sort(data, updateData, 0, data.Count - 1);
		}

		private void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData, int low, int high)
		{

			KeyValuePair<string, double> pivot;//存储分支点    
			int l, r;
			int mid;
			if (high <= low)
			{
				return;
			}

			if (high == (low + 1))
			{
				if (data[low].Value > data[high].Value)
				{
					swap(data, updateData, low, high);
				}
				return;
			}

			mid = (low + high) >> 1;
			pivot = data[mid];
			swap(data, updateData, low, mid);
			l = low + 1;
			r = high;

			do
			{
				while (l <= r && data[l].Value < pivot.Value)
				{
					l++;
				}

				while (data[r].Value >= pivot.Value)
				{
					r--;
					if (l < r)
					{
						swap(data, updateData, l, r);
					}
				}
			} while (l < r);

			data[low] = data[r];
			data[r] = pivot;
			if ((low + 1) < r)
			{
				Sort(data, updateData, low, (r - 1));
			}
			if ((r + 1) < high)
			{
				Sort(data, updateData, (r + 1), high);
			}
		}


		private void swap(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData, int change1Index, int change2Index)
		{
			var temp = data[change1Index];
			data[change1Index] = data[change2Index];
			data[change2Index] = temp;

			updateData(data);
		}
	}
}
