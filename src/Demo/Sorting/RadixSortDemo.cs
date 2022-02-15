using System;

namespace Sorting;

/// <summary>
///     基数排序（Radix Sort）：https://blog.csdn.net/qq_31116753/article/details/81633591
/// </summary>
internal class RadixSortDemo
{
    public static void Main(string[] args)
    {
        int[] array = { 43, 69, 11, 72, 28, 21, 56, 80, 48, 94, 32, 8 };

        RadixSort(array, 10);
        ShowSord(array);

        Console.ReadKey();
    }

    private static void ShowSord(int[] array)
    {
        foreach (var num in array) Console.Write($"{num} ");
        Console.WriteLine();
    }

    public static void RadixSort(int[] array, int bucketNum)
    {
        var maxLength = MaxLength(array);
        //创建bucket时，在二维中增加一组标识位，其中bucket[x, 0]表示这一维所包含的数字的个数
        //通过这样的技巧可以少写很多代码
        var bucket = new int[bucketNum, array.Length + 1];
        for (var i = 0; i < maxLength; i++)
        {
            foreach (var num in array)
            {
                var bit = (int)(num / Math.Pow(10, i) % 10);
                bucket[bit, ++bucket[bit, 0]] = num;
            }

            for (int count = 0, j = 0; j < bucketNum; j++)
            for (var k = 1; k <= bucket[j, 0]; k++)
                array[count++] = bucket[j, k];
            //最后要重置这个标识
            for (var j = 0; j < bucketNum; j++) bucket[j, 0] = 0;
        }
    }

    private static int MaxLength(int[] array)
    {
        if (array.Length == 0) return 0;
        var max = array[0];
        for (var i = 1; i < array.Length; i++)
            if (array[i] > max)
                max = array[i];
        var count = 0;
        while (max != 0)
        {
            max /= 10;
            count++;
        }

        return count;
        //return (int)Math.Log10(max) + 1;
    }
}