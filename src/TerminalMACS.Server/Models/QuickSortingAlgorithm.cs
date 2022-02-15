using System;
using System.Collections.Generic;

namespace TerminalMACS.Server.Models;

internal class QuickSortingAlgorithm : ISortingAlgorithm
{
    public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
    {
        QuickSort(data, updateData, 0, data.Count - 1);
    }

    private void QuickSort(
        List<KeyValuePair<string, double>> data,
        Action<List<KeyValuePair<string, double>>> updateData,
        int begin,
        int end)
    {
        // 两个指针重合就返回，结束调用
        if (begin >= end) return;

        //会得到一个基准值下标
        var pivotIndex = QuickSort_Once(data, updateData, begin, end);

        //对基准的左端进行排序  递归
        QuickSort(data, updateData, begin, pivotIndex - 1);

        //对基准的右端进行排序  递归
        QuickSort(data, updateData, pivotIndex + 1, end);
    }


    private int QuickSort_Once(
        List<KeyValuePair<string, double>> data,
        Action<List<KeyValuePair<string, double>>> updateData,
        int begin,
        int end)
    {
        //将首元素作为基准
        var pivot = data[begin];
        var i = begin;
        var j = end;

        while (i < j)
        {
            //从右到左，寻找第一个小于基准pivot的元素
            while (data[j].Value >= pivot.Value && i < j) j--; //指针向前移

            //执行到此，j已指向从右端起第一个小于基准pivot的元素，执行替换
            data[i] = data[j];
            updateData(data);

            //从左到右，寻找首个大于基准pivot的元素
            while (data[i].Value <= pivot.Value && i < j) i++; //指针向后移

            //执行到此,i已指向从左端起首个大于基准pivot的元素，执行替换
            data[j] = data[i];
            updateData(data);
        }

        //退出while循环,执行至此，必定是 i= j的情况（最后两个指针会碰头）
        //i(或j)所指向的既是基准位置，定位该趟的基准并将该基准位置返回
        data[i] = pivot;
        updateData(data);
        return i;
    }
}