using System;
using System.Collections.Generic;

namespace TerminalMACS.Server.Models;

/// <summary>
///     冒泡排序算法
/// </summary>
internal class EbullitionSortingAlgorithm : ISortingAlgorithm
{
    /// <summary>
    ///     按升序的方式排序
    /// </summary>
    /// <param name="data"></param>
    /// <param name="updateData"></param>
    public void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData)
    {
        for (var i = 0; i < data.Count; i++)
        for (var j = i + 1; j < data.Count; j++)
            if (data[i].Value > data[j].Value)
            {
                var temp = data[i];
                data[i] = data[j];
                data[j] = temp;
                updateData(data);
            }
    }
}