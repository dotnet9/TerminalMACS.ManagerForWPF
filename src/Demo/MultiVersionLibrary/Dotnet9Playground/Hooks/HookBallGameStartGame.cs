using Dotnet9Games.Views;
using Harmony;
using System.Reflection;

namespace Dotnet9Playground.Hooks;

internal class HookBallGameStartGame
{
    /// <summary>
    /// 拦截游戏的开始方法StartGame
    /// </summary>
    public static void StartHook()
    {
        var harmony = HarmonyInstance.Create("https://dotnet9.com/HookBallGameStartGame");
        var hookClassType = typeof(BallGame);
        var hookMethod =
            hookClassType!.GetMethod(nameof(BallGame.StartGame), BindingFlags.Public | BindingFlags.Instance);
        var replaceMethod = typeof(HookBallGameStartGame).GetMethod(nameof(HookStartGame));
        var replaceHarmonyMethod = new HarmonyMethod(replaceMethod);
        harmony.Patch(hookMethod, replaceHarmonyMethod);
    }

    /// <summary>
    /// StartGame替换方法
    /// </summary>
    /// <param name="__instance">BallGame实例</param>
    /// <returns></returns>
    public static bool HookStartGame(ref object __instance)
    {
        #region 原方法原代码

        //if (BallCount > 9)
        //{
        //    // 播放爆炸动画效果
        //    PlayExplosionAnimation();
        //}
        //else
        //{
        //    // 生成彩色气球
        //    GenerateBalloons();
        //}

        #endregion

        #region 拦截替换方法逻辑

        // 1、删除气球个数限制逻辑
        // 2、生成气球方法为private修饰，我们通过反射调用
        var instanceType = __instance.GetType();
        var hookGenerateBalloonsMethod =
            instanceType.GetMethod("GenerateBalloons", BindingFlags.Instance | BindingFlags.NonPublic);

        // 生成彩色气球
        hookGenerateBalloonsMethod!.Invoke(__instance, null);

        #endregion

        return false;
    }
}