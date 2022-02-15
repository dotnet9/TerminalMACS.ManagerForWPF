using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TerminalMACS.Controls.Controls;
using TerminalMACS.Server.Models;

namespace TerminalMACS.Server.Views;

/// <summary>
///     ChatTest.xaml 的交互逻辑
/// </summary>
public partial class ChatTest : UserControl
{
    private readonly List<KeyValuePair<string, double>> data = new();
    private readonly Dictionary<string, ISortingAlgorithm> dictSortingAlgorithms = new();
    private readonly List<WpfChart> lstSortCharts = new();
    private readonly List<TextBlock> lstSortNameTextBlocks = new();
    private readonly Random rd = new(DateTime.Now.Millisecond);
    private int sleepMs = 50;


    public ChatTest()
    {
        InitializeComponent();

        dictSortingAlgorithms["选择排序算法"] = new SelectionSortingAlgorithm();
        dictSortingAlgorithms["冒泡排序算法"] = new EbullitionSortingAlgorithm();
        dictSortingAlgorithms["快速排序算法"] = new QuickSortingAlgorithm();
        dictSortingAlgorithms["插入排序算法"] = new InsertionSortingAlgorithm();
        dictSortingAlgorithms["希尔排序"] = new ShellSortingAlgorithm();

        lstSortNameTextBlocks.Add(tbSortName1);
        lstSortNameTextBlocks.Add(tbSortName2);
        lstSortNameTextBlocks.Add(tbSortName3);
        lstSortNameTextBlocks.Add(tbSortName4);
        lstSortNameTextBlocks.Add(tbSortName5);

        lstSortCharts.Add(chart1);
        lstSortCharts.Add(chart2);
        lstSortCharts.Add(chart3);
        lstSortCharts.Add(chart4);
        lstSortCharts.Add(chart5);

        for (var i = 0; i < lstSortCharts.Count; i++)
            ChangeSoringAlgorithmName(lstSortNameTextBlocks[i], dictSortingAlgorithms.Keys.ToList()[i]);
    }

    private void Begin_Click(object sender, RoutedEventArgs e)
    {
        EnableUIBtn(false);
        lstSortCharts.ForEach(chart =>
        {
            var index = lstSortCharts.IndexOf(chart);
            var sortAlgorithms = dictSortingAlgorithms.Values.ToList()[index];
            ThreadPool.QueueUserWorkItem(sen =>
            {
                var tempData = CopyArray(data);
                sortAlgorithms.Sort(tempData, newDatas => { UpdateChart(chart, newDatas); });

                Thread.Sleep(TimeSpan.FromSeconds(2));
            });
            EnableUIBtn(true);
        });
    }

    /// <summary>
    ///     初始化数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void InitNumber_Click(object sender, RoutedEventArgs e)
    {
        data.Clear();

        var numberCount = 20;
        if (!int.TryParse(tbTestNumberCount.Text, out numberCount)) numberCount = 20;

        for (var i = 0; i < numberCount; i++)
            data.Add(new KeyValuePair<string, double>((i + 1).ToString(), rd.Next(5, 100)));

        if (!int.TryParse(tbSleepMs.Text, out var sleepMsTmp))
            sleepMs = 150;
        else
            sleepMs = sleepMsTmp;

        lstSortCharts.ForEach(cu => UpdateChart(cu, data));
    }

    /// <summary>
    ///     更新柱状图
    /// </summary>
    /// <param name="arrs"></param>
    private void UpdateChart(WpfChart chart, List<KeyValuePair<string, double>> arrs)
    {
        Dispatcher.Invoke(() =>
        {
            chart.ItemsSource = null;
            chart.ItemsSource = arrs;
        });
        Thread.Sleep(TimeSpan.FromMilliseconds(sleepMs));
    }

    private List<KeyValuePair<string, double>> CopyArray(List<KeyValuePair<string, double>> arrs)
    {
        var arrs2 = new List<KeyValuePair<string, double>>();
        foreach (var item in arrs) arrs2.Add(item);
        return arrs2;
    }

    private void EnableUIBtn(bool isEnabled)
    {
        Dispatcher.Invoke(() => { btnInitNumber.IsEnabled = btnBeginSort.IsEnabled = isEnabled; });
    }

    private void ChangeSoringAlgorithmName(TextBlock tb, string name)
    {
        Dispatcher.Invoke(() => { tb.Text = name; });
    }
}