using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using TerminalMACS.Server.Views;

namespace TerminalMACS.Server;

[ModuleDependency(ModuleNames.HOME)]
[Module(ModuleName = ModuleNames.Server)]
public class ServerModule : ModuleBase
{
    private readonly IRegionManager _regionManager;

    public ServerModule(IUnityContainer container, IRegionManager regionManager) : base(container)
    {
        _regionManager = regionManager;
    }

    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
        _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
    }
}