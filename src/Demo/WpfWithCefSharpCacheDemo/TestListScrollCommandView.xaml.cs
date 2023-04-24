using System.Windows;
using WpfWithCefSharpCacheDemo.TestListBoxScrollCommand;

namespace WpfWithCefSharpCacheDemo;

public partial class TestListScrollCommandView : Window
{
    public TestListScrollCommandView()
    {
        this.DataContext = new TestListScrollCommandViewModel();
        InitializeComponent();
    }
}