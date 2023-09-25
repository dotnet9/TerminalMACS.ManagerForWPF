using Dotnet9Games.Views;
using HarmonyLib;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

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
        var harmony = new Harmony("https://dotnet9.com/HookBallgameMeasureOverride");
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
        #region 原方法代码逻辑

        //// 计算最后一个元素宽度，不需要关注为什么这样写，只是为了引出Size异常使得

        //var lastChild = _balloons.LastOrDefault();
        //if (lastChild != null)
        //{
        //    var remainWidth = ActualWidth;
        //    foreach (var balloon in _balloons)
        //    {
        //        remainWidth -= balloon.Shape.Width;
        //    }

        //    lastChild.Shape.Measure(new Size(remainWidth, lastChild.Shape.Height));
        //}

        //return base.MeasureOverride(constraint);

        #endregion

        #region 拦截替换代码

        var instanceType = __instance.GetType();
        var balloonsField = instanceType.GetField("_balloons", BindingFlags.NonPublic | BindingFlags.Instance);
        var balloons = (IEnumerable)balloonsField!.GetValue(__instance);

        var lastChild = balloons.Cast<object>().LastOrDefault();
        if (lastChild == null)
        {
            return false;
        }

        var remainWidth = ((UserControl)__instance).ActualWidth;
        foreach (object balloon in balloons)
        {
            remainWidth -= GetBalloonSize(balloon).Width;
        }

        // 注意：关键代码在这，如果剩余宽度大于0才重新计算最后一个子项大小
        // 这段代码可能没什么意义，可按实际开发修改
        if (remainWidth > 0)
        {
            var lashShape = GetBalloonShape(lastChild);
            lashShape.Measure(new Size(remainWidth, lashShape.Height));
        }

        #endregion

        return false;
    }

    private static Ellipse GetBalloonShape(object balloon)
    {
        var shapeProperty = balloon.GetType().GetProperty("Shape");
        var shape = (Ellipse)shapeProperty!.GetValue(balloon);
        return shape;
    }

    private static Size GetBalloonSize(object balloon)
    {
        var shape = GetBalloonShape(balloon);
        return new Size(shape.Width, shape.Height);
    }
}