using HarmonyLib;
using System.Reflection;
using TestDll;

namespace MultiVersionLibrary;

/// <summary>
/// HarmonyPatch手动拦截的类及方法
/// </summary>
internal class HookGetValidNumber
{
    /// <summary>
    /// 手工注册关联被拦截方法与替换方法
    /// </summary>
    public static void StartHook()
    {
        var harmony = new Harmony("https://dotnet9.com");
        var hookClassType = typeof(TestTool).Assembly.GetType("TestDll.CalNumber");
        var hookMethod = hookClassType!.GetMethod("GetValidNumber", BindingFlags.NonPublic | BindingFlags.Instance,
            new[] { typeof(int) });
        var replaceMethod = typeof(HookGetValidNumber).GetMethod(nameof(Prefix));
        var replaceHarmonyMethod = new HarmonyMethod(replaceMethod);
        harmony.Patch(hookMethod, replaceHarmonyMethod);
    }

    /// <summary>
    /// GetNumberSentence拦截替换方法
    /// </summary>
    /// <param name="__instance">拦截的TestTool实例</param>
    /// <param name="number">GetNumberSentence方法同名参数定义，修改它达到方法参数篡改</param>
    /// <param name="__result">GetNumberSentence方法返回值，修改它达到方法值伪造</param>
    /// <returns></returns>
    public static bool Prefix(ref object __instance, int number, ref int __result)
    {
        try
        {
            //将原方法逻辑全部复制，然后做部分修改

            // 这里可以加一些复杂的算法代码
            if (number == 6)
            {
                number = 8;
            }

            __result = number;

            return false;
        }
        catch (Exception ex)
        {
            return true;
        }
    }
}