namespace Sorting;

/// <summary>
///     选择排序（Selection Sort）
/// </summary>
internal class SelectionSortDemo
{
    //static void Main(string[] args)
    //{
    //	int[] arr = { 3, 5, 8, 1, 0 };
    //	Console.WriteLine(string.Join(',', SelectionSort(arr)));
    //}

    public static int[] SelectionSort(int[] arr)
    {
        var len = arr.Length;

        int minIndex, temp;

        for (var i = 0; i < len - 1; i++)
        {
            minIndex = i;

            for (var j = i + 1; j < len; j++)
                // 寻找最小的数
                if (arr[j] < arr[minIndex])
                    // 将最小数的索引保存
                    minIndex = j;
            temp = arr[i];
            arr[i] = arr[minIndex];
            arr[minIndex] = temp;
        }

        return arr;
    }
}