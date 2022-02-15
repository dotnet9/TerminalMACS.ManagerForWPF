namespace Sorting;

/// <summary>
///     插入排序（Insertion Sort）
/// </summary>
internal class InsertionSortDemo
{
    //static void Main(string[] args)
    //{
    //	int[] arr = { 3, 5, 8, 1, 0 };
    //	Console.WriteLine(string.Join(',', InsertionSort(arr)));
    //}

    public static int[] InsertionSort(int[] arr)
    {
        var len = arr.Length;

        int preIndex, current;
        for (var i = 1; i < len; i++)
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