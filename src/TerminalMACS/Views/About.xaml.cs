using System.Reflection;
using AduSkin.Controls.Metro;

namespace TerminalMACS.Views;

public partial class About : MetroWindow
{
    public About()
    {
        InitializeComponent();

        this.txtVersion.Text = $"v {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
    }
}