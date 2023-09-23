using HarmonyLib;
using System.Reflection;
using TestDll;

namespace MultiVersionLibrary;

/// <summary>
/// HarmonyPatch特性关联拦截的类及方法
/// </summary>
[HarmonyPatch(typeof(TestTool))]
[HarmonyPatch(nameof(TestTool.GetNumberSentence))]
[HarmonyPatch(new Type[] { typeof(int) })]
internal class HookGetNumberSentence
{
    /// <summary>
    /// GetNumberSentence拦截替换方法
    /// </summary>
    /// <param name="__instance">拦截的TestTool实例</param>
    /// <param name="number">GetNumberSentence方法同名参数定义，修改它达到方法参数篡改</param>
    /// <param name="__result">GetNumberSentence方法返回值，修改它达到方法值伪造</param>
    /// <returns></returns>
    public static bool Prefix(ref object __instance, int number, ref string __result)
    {
        try
        {
            //将原方法逻辑全部复制，然后做部分修改

            //1、_sentences是拦截类TestTool的私胡字段，我们通过反射获取值
            var sentences =
                __instance.GetType().GetField("_sentences", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(__instance) as List<string>;
            if (sentences?.Any() != true)
            {
                __result = "啊，没有优美句子吗？";
                return true;
            }

            var mo = number % sentences.Count;

            // 个位为0，取最后一
            if (mo == 0)
            {
                mo = 10;
            }

            // 2、注释我们认为有歧义的代码
            //if (mo == 6)
            //{
            //    mo = 1;
            //}

            var sentencesIndex = mo - 1;
            __result = sentences[sentencesIndex];

            return false;
        }
        catch (Exception ex)
        {
            return true;
        }
    }
}