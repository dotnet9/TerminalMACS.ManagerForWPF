using HarmonyLib;
using System.Xml.Linq;

var dotnet9Domain = "https://dotnet9.com";
Console.WriteLine($"9的位置：{dotnet9Domain.IndexOf('9', 0)}");

var harmony = new Harmony("com.dotnet9");
harmony.PatchAll();

Console.WriteLine($"9的位置：{dotnet9Domain.IndexOf('9', 0)}");
Console.WriteLine(new PointHelper().GetPoint("名称"));

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

[HarmonyPatch(typeof(PointHelper))]
[HarmonyPatch(nameof(PointHelper.GetPoint))]
public static class HookPointHelper
{
    public static bool Prefix(ref string name, ref Point __result)
    {
        name = "test";
        __result = new Point() { X = 2, Y = 3 };
        return false;
    }
}

public class PointHelper
{
    public Point GetPoint(string name)
    {
        Console.WriteLine($"收到参数：{name}");
        return new Point { X = 100, Y = 100 };
    }

    public string SetPoint(Point point)
    {
        Console.WriteLine($"收到参数,x={point.X},y={point.Y}");
        return "test";
    }
}

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"x={X},y={Y}";
    }
}