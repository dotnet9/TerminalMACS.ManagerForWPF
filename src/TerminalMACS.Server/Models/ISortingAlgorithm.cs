using System;
using System.Collections.Generic;

namespace TerminalMACS.Server.Models;

internal interface ISortingAlgorithm
{
    void Sort(List<KeyValuePair<string, double>> data, Action<List<KeyValuePair<string, double>>> updateData);
}