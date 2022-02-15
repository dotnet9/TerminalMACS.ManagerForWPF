namespace Sorting;

/// <summary>
///     希尔排序（Shell Sort）
/// </summary>
internal class ShellSortDemo
{
    //public static void Main(string[] args)
    //{
    //	int[] arr = { 3, 5, 8, 1, 0 };
    //	Console.WriteLine(string.Join(',', ShellSort(arr)));
    //}

    public static int[] ShellSort(int[] data)
    {
        var j = 0;
        var temp = 0;
        for (var increment = data.Length / 2; increment > 0; increment /= 2)
        for (var i = increment; i < data.Length; i++)
        {
            temp = data[i];
            for (j = i - increment; j >= 0; j -= increment)
                if (temp < data[j])
                    data[j + increment] = data[j];
                else
                    break;
            data[j + increment] = temp;
        }

        return data;
    }
}