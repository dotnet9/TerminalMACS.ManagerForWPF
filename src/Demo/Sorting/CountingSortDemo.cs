namespace Sorting;

/// <summary>
///     计数排序（Counting Sort）,参考：https://lovejy.blog.csdn.net/article/details/81610602
/// </summary>
internal class CountingSortDemo
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
        var min = array[0];
        var max = min;
        foreach (var number in array)
            if (number > max)
                max = number;
            else if (number < min) min = number;
        var counting = new int[max - min + 1];
        for (var i = 0; i < array.Length; i++) counting[array[i] - min] += 1;
        var index = -1;
        for (var i = 0; i < counting.Length; i++)
        for (var j = 0; j < counting[i]; j++)
        {
            index++;
            array[index] = i + min;
        }
    }
}