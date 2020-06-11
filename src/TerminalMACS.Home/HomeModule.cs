using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TerminalMACS.Home.Views;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Infrastructure.UI.Modularity;
using Unity;
using WpfExtensions.Xaml;

namespace TerminalMACS.Home
{
    [Module(ModuleName = ModuleNames.HOME)]
    public class HomeModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;
        public HomeModule(IUnityContainer container, IRegionManager regionManager) : base(container)
        {
            I18nManager.Instance.Add(TerminalMACS.Home.I18nResources.UiResource.ResourceManager);
            _regionManager = regionManager;
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainTabRegion, typeof(MainTabItem));
            _regionManager.RegisterViewWithRegion(RegionNames.SettingsTabRegion, typeof(SettingsTabItem));
        }
    }
}