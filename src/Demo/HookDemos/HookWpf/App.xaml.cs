using HarmonyLib;
using System.Windows;

namespace HookWpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var harmony = new Harmony("com.dotnet9");
        harmony.PatchAll();
        MessageBox.Show("内容", "标题");
        this.Show();
    }

    public void Show()
    {
        MessageBox.Show("测试提示");
    }
}