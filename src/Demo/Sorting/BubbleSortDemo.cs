namespace Sorting;

/// <summary>
///     冒泡排序（Bubble Sort）
/// </summary>
internal class BubbleSortDemo
{
    //static void Main(string[] args)
    //{
    //	int[] arr = { 3, 5, 8, 1, 0 };
    //	Console.WriteLine(string.Join(',', BubbleSort(arr)));
    //}

    public static int[] BubbleSort(int[] arr)
    {
        var temp = 0;
        for (var i = 0; i < arr.Length - 1; i++)
        for (var j = 0; j < arr.Length - 1 - i; j++)
            if (arr[j] > arr[j + 1])
            {
                temp = arr[j + 1];
                arr[j + 1] = arr[j];
                arr[j] = temp;
            }

        return arr;
    }
}