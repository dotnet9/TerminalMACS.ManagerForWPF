using NodeNetwork;
using System.Windows;

namespace NodeNetworkTest;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        NNViewRegistrar.RegisterSplat();
    }
}