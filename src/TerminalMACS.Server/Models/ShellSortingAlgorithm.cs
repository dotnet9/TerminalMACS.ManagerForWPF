using System;
using System.Collections.Generic;

namespace TerminalMACS.Server.Models;

/// <summary>
///     希尔排序
/// </summary>
internal class ShellSortingAlgorithm : ISortingAlgorithm
{
    public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
    {
        int inc;
        for (inc = 1; inc <= data.Count / 9; inc = 3 * inc + 1) ;
        for (; inc > 0; inc /= 3)
        for (var i = inc + 1; i <= data.Count; i += inc)
        {
            var t = data[i - 1];
            var j = i;
            while (j > inc && data[j - inc - 1].Value > t.Value)
            {
                data[j - 1] = data[j - inc - 1]; //交换数据    
                updateData(data);
                j -= inc;
            }

            data[j - 1] = t;
            updateData(data);
        }
    }
}