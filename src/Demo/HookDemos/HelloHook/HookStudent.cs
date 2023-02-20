using HarmonyLib;

namespace HelloHook;

[HarmonyPatch(typeof(Student))]
[HarmonyPatch(nameof(Student.GetDetails))]
public class HookStudent
{
    public static bool Prefix(ref string name, ref string __result)
    {
        if ("沙漠之狐".Equals(name))
        {
            __result = $"这是我的曾用网名";
            return false;
        }

        if (!"沙漠尽头的狼".Equals(name))
        {
            name = "非站长名";
        }

        return true;
    }
}