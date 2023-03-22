using DataGridDemo.Views;
using Prism.Ioc;
using System.Windows;

namespace DataGridDemo;

public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<StudentView>();
    }
}