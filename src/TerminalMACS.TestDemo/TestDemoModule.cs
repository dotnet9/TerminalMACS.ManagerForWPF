using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using TerminalMACS.TestDemo.Views;

namespace TerminalMACS.TestDemo;

[ModuleDependency(ModuleNames.HOME)]
[Module(ModuleName = ModuleNames.TestDemo)]
public class TestDemoModule : ModuleBase
{
    private readonly IRegionManager _regionManager;

    public TestDemoModule(IUnityContainer container, IRegionManager regionManager) : base(container)
    {
        _regionManager = regionManager;
    }

    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
        _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
    }
}