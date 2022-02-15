using System;

namespace Sorting;

/// <summary>
///     归并排序（Merge Sort）
/// </summary>
internal class MergeSortDemo
{
    public static void Merge(int[] arr, int low, int mid, int high)
    {
        var temp = new int[high - low + 1];
        var i = low; // 左指针
        var j = mid + 1; // 右指针
        var k = 0;
        // 把较小的数先移到新数组中
        while (i <= mid && j <= high)
            if (arr[i] < arr[j])
                temp[k++] = arr[i++];
            else
                temp[k++] = arr[j++];
        // 把左边剩余的数移入数组
        while (i <= mid) temp[k++] = arr[i++];
        // 把右边边剩余的数移入数组
        while (j <= high) temp[k++] = arr[j++];
        // 把新数组中的数覆盖nums数组
        for (var k2 = 0; k2 < temp.Length; k2++) arr[k2 + low] = temp[k2];
    }

    public static void MergeSort(int[] arr, int low, int high)
    {
        var mid = (low + high) / 2;
        if (low < high)
        {
            // 左边
            MergeSort(arr, low, mid);
            // 右边
            MergeSort(arr, mid + 1, high);
            // 左右归并
            Merge(arr, low, mid, high);
            Console.WriteLine(string.Join(',', arr));
        }
    }

    //public static void Main(string[] args)
    //{
    //	int[] arr = { 51, 46, 20, 18, 65, 97, 82, 30, 77, 50 };
    //	MergeSort(arr, 0, arr.Length - 1);
    //	Console.WriteLine(string.Join(',', arr));
    //}
}