using Dotnet9Games.Views;
using HarmonyLib;
using System.Reflection;

namespace Dotnet9HookHigh;

/// <summary>
/// 拦截BallGame的MeasureOverride方法
/// </summary>
public class HookBallgameMeasureOverride
{
    /// <summary>
    /// 拦截游戏的MeasureOverride方法
    /// </summary>
    public static void StartHook()
    {
        //var harmony =  HarmonyInstance.Create("https://dotnet9.com/HookBallgameMeasureOverride");
        // 上面是低版本Harmony实例获取代码，下面是高版本
        var harmony =  new Harmony("https://dotnet9.com/HookBallgameMeasureOverride");
        var hookClassType = typeof(BallGame);
        var hookMethod = hookClassType!.GetMethod("MeasureOverride", BindingFlags.NonPublic | BindingFlags.Instance);
        var replaceMethod = typeof(HookBallgameMeasureOverride).GetMethod(nameof(HookMeasureOverride));
        var replaceHarmonyMethod = new HarmonyMethod(replaceMethod);
        harmony.Patch(hookMethod, replaceHarmonyMethod);
    }

    /// <summary>
    /// MeasureOverride替换方法
    /// </summary>
    /// <param name="__instance">BallGame实例</param>
    /// <returns></returns>
    public static bool HookMeasureOverride(ref object __instance)
    {
        return false;
    }
}