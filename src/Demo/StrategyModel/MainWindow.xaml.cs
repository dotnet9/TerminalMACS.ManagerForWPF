using System;
using System.Windows;

namespace StrategyModel
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();
			cbxType.Items.Add("正常收费");
			cbxType.Items.Add("打八折");
			cbxType.Items.Add("打七折");
			cbxType.Items.Add("打五折");
			cbxType.SelectedIndex = 0;
		}

		double total = 0.0d;

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			// 利用简单工厂模式根据下拉选择框，生成相应的对象
			CashSuper csuper = CashFactory.CreateCashAccept(cbxType.SelectedItem.ToString());
			double totalPrices = 0d;

			// 通过多态，可以得到收取费用的结果
			totalPrices = csuper.AcceptCash(Convert.ToDouble(txtPrice.Text)*Convert.ToDouble(txtNum.Text));
			total = total + totalPrices;

			lbxList.Items.Add($"单价：{txtPrice.Text} 数量：{txtNum.Text} {cbxType.SelectedItem} 合计：{totalPrices}");

			lblResult.Text = total.ToString();
		}
	}
}
