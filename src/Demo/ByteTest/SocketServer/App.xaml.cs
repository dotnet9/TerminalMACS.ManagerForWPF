using Prism.DryIoc;
using System.Windows;

namespace SocketServer;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainView>();
    }
}