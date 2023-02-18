using System.Windows;
using HarmonyLib;

namespace HookWpf;

[HarmonyPatch(typeof(MessageBox))]
[HarmonyPatch(nameof(MessageBox.Show))]
[HarmonyPatch(new [] { typeof(string), typeof(string) })]
public class HookMessageBox
{
    public static bool Prefix(ref string messageBoxText, string caption)
    {
        if (messageBoxText.Contains("垃圾"))
        {
            messageBoxText = "这是一个不错的网站哟";
        }

        return true;
    }
}

[HarmonyPatch(typeof(App))]
[HarmonyPatch(nameof(App.Show))]
public class HookAppShow
{
    public static bool Prefix()
    {

        return true;
    }
}