using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.IO;
using System.Windows;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Views;
using WpfExtensions.Xaml;

namespace TerminalMACS
{
    public partial class App : PrismApplication
    {
        App()
        {
        }
        protected override Window CreateShell()
        {
            I18nManager.Instance.Add(TerminalMACS.I18nResources.UiResource.ResourceManager);
            LanguageHelper.SetLanguage();
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            string modulePath = @".\Modules";
            if (!Directory.Exists(modulePath))
            {
                Directory.CreateDirectory(modulePath);
            }
            return new DirectoryModuleCatalog() { ModulePath = modulePath };
        }
    }
}
