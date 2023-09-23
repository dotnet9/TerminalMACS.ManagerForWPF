using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using HarmonyLib;
using MultiVersionLibrary.ViewModels;
using MultiVersionLibrary.Views;
using System.Reflection;

namespace MultiVersionLibrary;

public partial class App : Application
{
    public override void Initialize()
    {
        // 1、自动注册拦截类：拦截类上添加被拦截类和方法特性
        var harmony = new Harmony("https://dotnet9.com");
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        // 2、自动注册拦截类，构造被拦截类和方法信息进行拦截
        HookGetValidNumber.StartHook();

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        Console.WriteLine();
        base.OnFrameworkInitializationCompleted();
    }
}