using HarmonyLib;

var dotnet9Domain = "https://dotnet9.com";
Console.WriteLine($"9的位置：{dotnet9Domain.IndexOf('9',0)}");

var harmony = new Harmony("com.dotnet9");
harmony.PatchAll();

Console.WriteLine($"9的位置：{dotnet9Domain.IndexOf('9', 0)}");

[HarmonyPatch(typeof(String))]
[HarmonyPatch(nameof(string.IndexOf))]
[HarmonyPatch(new Type[] { typeof(char), typeof(int) })]
public static class HookClass
{
    public static bool Prefix(ref int __result)
    {
        __result = 100;
        return false;
    }
}