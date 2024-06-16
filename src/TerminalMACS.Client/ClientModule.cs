using TerminalMACS.Client.I18nResources;
using TerminalMACS.Client.Views;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using WpfExtensions.Xaml;

namespace TerminalMACS.Client;

[ModuleDependency(ModuleNames.HOME)]
[ModuleDependency(ModuleNames.Server)]
[Module(ModuleName = ModuleNames.Client)]
public class ClientModule : ModuleBase
{
    private readonly IRegionManager _regionManager;

    public ClientModule(IUnityContainer container, IRegionManager regionManager) : base(container)
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