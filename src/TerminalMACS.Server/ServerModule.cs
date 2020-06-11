using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using TerminalMACS.Server.Views;
using Unity;
using WpfExtensions.Xaml;

namespace TerminalMACS.Server
{
    [ModuleDependency(ModuleNames.HOME)]
    [Module(ModuleName = ModuleNames.Server)]
    public class ServerModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;
        public ServerModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            I18nManager.Instance.Add(TerminalMACS.Server.I18nResources.UiResource.ResourceManager);
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
            _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
        }
    }
}