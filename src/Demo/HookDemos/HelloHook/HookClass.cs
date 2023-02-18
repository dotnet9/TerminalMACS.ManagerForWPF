using HarmonyLib;
using HookDemos.OwnerLibrary;

namespace HelloHook;

[HarmonyPatch(typeof(Student))] // 定义拦截的类类型：Student
[HarmonyPatch(nameof(Student.GetDetails))] // 定义拦截的方法：GetDetail
public class HookClass
{
    public static bool Prefix(ref string name)
    {
        Console.WriteLine($"Prefix");
        if (!"沙漠尽头的狼".Equals(name))
        {
            name = "假的，千万别信";
        }

        return true;
    }

    public static void Postfix(string name, ref string __result)
    {
        __result = $"不确定'{name}'是哪个网站的站长";
        Console.WriteLine($"Postfix");
    }

    public static void Finalizer()
    {
        Console.WriteLine($"Finalizer");
    }
}