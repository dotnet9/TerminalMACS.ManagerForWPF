using System.IO;
using System.Windows;
using TerminalMACS.Infrastructure.Services;
using TerminalMACS.Infrastructure.UI;
using TerminalMACS.Views;

namespace TerminalMACS;

public partial class App : PrismApplication
{
    private App()
    {
    }

    protected override Window CreateShell()
    {
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