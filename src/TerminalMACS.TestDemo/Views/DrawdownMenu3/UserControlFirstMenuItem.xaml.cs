using System.Windows.Controls;

namespace TerminalMACS.TestDemo.Views.DrawdownMenu3
{
    /// <summary>
    /// UserControlFirstMenuItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlFirstMenuItem : UserControl
    {
        public UserControlFirstMenuItem(string name)
        {
            InitializeComponent();
            this.txtName.Text = name;
        }
    }
}