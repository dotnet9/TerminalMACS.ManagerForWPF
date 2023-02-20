using HarmonyLib;
using System.Windows;

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