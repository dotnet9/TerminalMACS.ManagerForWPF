using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using TerminalMACS.TestDemo.I18nResources;
using TerminalMACS.TestDemo.Views;
using Unity;
using WpfExtensions.Xaml;

namespace TerminalMACS.TestDemo;

[ModuleDependency(ModuleNames.HOME)]
[Module(ModuleName = ModuleNames.TestDemo)]
public class TestDemoModule : ModuleBase
{
    private readonly IRegionManager _regionManager;

    public TestDemoModule(IUnityContainer container, IRegionManager regionManager) : base(container)
    {
        I18nManager.Instance.Add(UiResource.ResourceManager);
        _regionManager = regionManager;
    }

    public override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
        _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
    }
}