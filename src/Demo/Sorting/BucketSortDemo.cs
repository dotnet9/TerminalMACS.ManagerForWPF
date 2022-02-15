using System;

namespace Sorting;

/// <summary>
///     桶排序（Bucket Sort）：https://blog.csdn.net/qq_31116753/article/details/81713640
/// </summary>
internal class BucketSortDemo
{
    //public static void Main(string[] args)
    //{
    //    double[] array = { 0.43, 0.69, 0.11, 0.72, 0.28, 0.21, 0.56, 0.80, 0.48, 0.94, 0.32, 0.08 };

    //    BucketSort(array, 10);
    //    ShowSord(array);

    //    Console.ReadKey();
    //}

    private static void ShowSord(double[] array)
    {
        foreach (var num in array) Console.Write($"{num} ");
        Console.WriteLine();
    }

    public static void BucketSort(double[] array, int bucketNum)
    {
        //创建bucket时，在二维中增加一组标识位，其中bucket[x, 0]表示这一维所包含的数字的个数
        //通过这样的技巧可以少写很多代码
        var bucket = new double[bucketNum, array.Length + 1];
        foreach (var num in array)
        {
            var bit = (int)(10 * num);
            bucket[bit, (int)++bucket[bit, 0]] = num;
        }

        //为桶里的每一行使用插入排序
        for (var j = 0; j < bucketNum; j++)
        {
            //为桶里的行创建新的数组后使用插入排序
            var insertion = new double[(int)bucket[j, 0]];
            for (var k = 0; k < insertion.Length; k++) insertion[k] = bucket[j, k + 1];
            //插入排序
            StraightInsertionSort(insertion);
            //把排好序的结果回写到桶里
            for (var k = 0; k < insertion.Length; k++) bucket[j, k + 1] = insertion[k];
        }

        //将所有桶里的数据回写到原数组中
        for (int count = 0, j = 0; j < bucketNum; j++)
        for (var k = 1; k <= bucket[j, 0]; k++)
            array[count++] = bucket[j, k];
    }

    public static void StraightInsertionSort(double[] array)
    {
        //插入排序
        for (var i = 1; i < array.Length; i++)
        {
            var sentinel = array[i];
            var j = i - 1;
            while (j >= 0 && sentinel < array[j])
            {
                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = sentinel;
        }
    }
}