using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TerminalMACS.Server.Models;

namespace TerminalMACS.Server.Views
{
	/// <summary>
	/// ChatTest.xaml 的交互逻辑
	/// </summary>
	public partial class ChatTest : UserControl
	{
		List<KeyValuePair<string, double>> data = new List<KeyValuePair<string, double>>();
		Dictionary<string, ISortingAlgorithm> dictSortingAlgorithms = new Dictionary<string, ISortingAlgorithm>();
		Random rd = new Random(DateTime.Now.Millisecond);
		public ChatTest()
		{
			InitializeComponent();

			//dictSortingAlgorithms["选择排序算法"] = new SelectionSortingAlgorithm();
			//dictSortingAlgorithms["冒泡排序算法"] = new EbullitionSortingAlgorithm();
			//dictSortingAlgorithms["快速排序算法"] = new QuickSortingAlgorithm();
			//dictSortingAlgorithms["插入排序算法"] = new InsertionSortingAlgorithm();
			dictSortingAlgorithms["希尔排序"] = new ShellSortingAlgorithm();
		}

		private void Begin_Click(object sender, RoutedEventArgs e)
		{
			this.EnableUIBtn(false);
			ThreadPool.QueueUserWorkItem(sen =>
			{
				foreach (var kvp in dictSortingAlgorithms)
				{
					this.ChangeSoringAlgorithmName(kvp.Key);
					var tempData = CopyArray(data);
					kvp.Value.Sort(tempData, this.UpdateChart);

					Thread.Sleep(TimeSpan.FromSeconds(2));
				}
				this.EnableUIBtn(true);
			});
		}

		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InitNumber_Click(object sender, RoutedEventArgs e)
		{
			data.Clear();

			int numberCount = 20;
			if (!int.TryParse(this.tbTestNumberCount.Text, out numberCount))
			{
				numberCount = 20;
			}
			for (int i = 0; i < numberCount; i++)
			{
				data.Add(new KeyValuePair<string, double>((i + 1).ToString(), rd.Next(5, 100)));
			}
			UpdateChart(data);
		}

		/// <summary>
		/// 更新柱状图
		/// </summary>
		/// <param name="arrs"></param>
		private void UpdateChart(List<KeyValuePair<string, double>> arrs)
		{
			this.Dispatcher.Invoke(() =>
			{
				this.chart.ItemsSource = null;
				this.chart.ItemsSource = arrs;
			});
			Thread.Sleep(TimeSpan.FromMilliseconds(500));
		}

		private List<KeyValuePair<string, double>> CopyArray(List<KeyValuePair<string, double>> arrs)
		{
			List<KeyValuePair<string, double>> arrs2 = new List<KeyValuePair<string, double>>();
			foreach (var item in arrs)
			{
				arrs2.Add(item);
			}
			return arrs2;
		}

		private void EnableUIBtn(bool isEnabled)
		{
			this.Dispatcher.Invoke(() =>
			{
				this.btnInitNumber.IsEnabled = this.btnBeginSort.IsEnabled = isEnabled;
			});
		}

		private void ChangeSoringAlgorithmName(string name)
		{
			this.Dispatcher.Invoke(() =>
			{
				this.tbSortingAlgorithmName.Text = name;
			});
		}
	}
}
