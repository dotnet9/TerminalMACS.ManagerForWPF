using HarmonyLib;
using HookDemos.OwnerLibrary;

namespace HelloHook;

[HarmonyPatch(typeof(Student))] // 定义拦截的类类型：Student
[HarmonyPatch(nameof(Student.GetDetail))] // 定义拦截的方法：GetDetail
public class HookClass
{
    /// <summary>
    /// 定义方法执行返回结果前的操作，在这里可以对拦截的方法参数进行篡改
    /// </summary>
    /// <param name="name">[这是可选的]，参数名与需要拦截的方法参数需要一致</param>
    /// <returns></returns>
    public static bool Prefix(ref string name)
    {
        name = "无名"; // 这里对参数进行了修改
        return true; // 返回true：将执行拦截方法，返回false：则原生方法不会执行，且拦截参数的篡改也不会生效
    }

    /// <summary>
    /// 定义方法执行返回结果的操作，在这里可以对方法返回的结果进行直接篡改
    /// </summary>
    /// <param name="__result">[这是可选的]，方法的返回结果，注意命名必须是__result</param>
    public static void Postfix(ref string __result)
    {
        __result = "No";  // 对结果进行篡改，将只输出：No
    }
}