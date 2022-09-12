using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TerminalMACS.TestDemo.Views.DrawdownMenu3
{
    /// <summary>
    /// DrawdownMenu3Window.xaml 的交互逻辑
    /// </summary>
    public partial class DrawdownMenu3Window : Window
    {
        public DrawdownMenu3Window()
        {
            InitializeComponent();
            var menuRegister = new List<SubItem>();
            menuRegister.Add(new SubItem("客户", new UserControlCustomers()));
            menuRegister.Add(new SubItem("供应商", new UserControlProviders()));
            menuRegister.Add(new SubItem("员工", new UserControlFirstMenuItem("员工")));
            menuRegister.Add(new SubItem("产品", new UserControlFirstMenuItem("产品")));
            var item6 = new ItemMenu("登记", menuRegister, PackIconKind.Register, new UserControlFirstMenuItem("登记"));

            var menuSchedule = new List<SubItem>();
            menuSchedule.Add(new SubItem("服务", new UserControlFirstMenuItem("服务")));
            menuSchedule.Add(new SubItem("会议", new UserControlFirstMenuItem("会议")));
            var item1 = new ItemMenu("预约", menuSchedule, PackIconKind.Schedule, new UserControlFirstMenuItem("预约"));

            var menuReports = new List<SubItem>();
            menuReports.Add(new SubItem("客户", new UserControlFirstMenuItem("客户")));
            menuReports.Add(new SubItem("供应商", new UserControlFirstMenuItem("供应商")));
            menuReports.Add(new SubItem("产品", new UserControlFirstMenuItem("产品")));
            menuReports.Add(new SubItem("库存", new UserControlFirstMenuItem("库存")));
            menuReports.Add(new SubItem("销售额", new UserControlFirstMenuItem("销售额")));
            var item2 = new ItemMenu("报告", menuReports, PackIconKind.FileReport, new UserControlFirstMenuItem("报告"));

            var menuExpenses = new List<SubItem>();
            menuExpenses.Add(new SubItem("固定资产", new UserControlFirstMenuItem("固定资产")));
            menuExpenses.Add(new SubItem("流动资金", new UserControlFirstMenuItem("流动资金")));
            var item3 = new ItemMenu("费用", menuExpenses, PackIconKind.ShoppingBasket,
                new UserControlFirstMenuItem("费用"));

            var menuFinancial = new List<SubItem>();
            menuFinancial.Add(new SubItem("现金流", new UserControlFirstMenuItem("现金流")));
            var item4 = new ItemMenu("财务", menuFinancial, PackIconKind.ScaleBalance,
                new UserControlFirstMenuItem("财务"));

            Menu.Children.Add(new UserControlMenuItem(item6, this));
            Menu.Children.Add(new UserControlMenuItem(item1, this));
            Menu.Children.Add(new UserControlMenuItem(item2, this));
            Menu.Children.Add(new UserControlMenuItem(item3, this));
            Menu.Children.Add(new UserControlMenuItem(item4, this));
        }

        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);

            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }
    }
}