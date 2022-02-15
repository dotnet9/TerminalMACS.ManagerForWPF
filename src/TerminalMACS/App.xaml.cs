using System.IO;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using TerminalMACS.I18nResources;
using TerminalMACS.Infrastructure.Services;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Views;
using WpfExtensions.Xaml;

namespace TerminalMACS;

public partial class App : PrismApplication
{
    private App()
    {
    }

    protected override Window CreateShell()
    {
        I18nManager.Instance.Add(UiResource.ResourceManager);
        LanguageHelper.SetLanguage();
        return Container.Resolve<CusSplashScreen>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<ITestService, TestService>();
    }

    protected override IModuleCatalog CreateModuleCatalog()
    {
        var modulePath = @".\Modules";
        if (!Directory.Exists(modulePath)) Directory.CreateDirectory(modulePath);
        return new DirectoryModuleCatalog { ModulePath = modulePath };
    }
}