using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace NewbieGuideDemo
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}
