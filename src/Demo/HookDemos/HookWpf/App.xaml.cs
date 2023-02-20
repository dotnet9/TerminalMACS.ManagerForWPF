using System.Reflection;
using HarmonyLib;
using System.Windows;

namespace HookWpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {

        base.OnStartup(e);

        var harmony = new Harmony("https://dotnet9.com");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}