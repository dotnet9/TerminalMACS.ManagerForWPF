using System.Reflection;
using HarmonyLib;
using System.Windows;

namespace HookWpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var messageBox = typeof(MessageBox).GetMethod(nameof(MessageBox.Show),
            BindingFlags.Public | BindingFlags.Static, new[] { typeof(string), typeof(string) });
        messageBox.Invoke(null, new object?[]{"内容", "标题"});

        base.OnStartup(e);
        var harmony = new Harmony("com.dotnet9");
        harmony.PatchAll();
        MessageBox.Show("内容", "标题");
        Show();
    }

    public static void Show()
    {
        MessageBox.Show("测试提示");
    }
}