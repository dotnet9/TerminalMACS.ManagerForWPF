using System.Reflection;

namespace Dotnet9Playground.Extensions;

internal static class PropertyHelper
{
    /// <summary>
    /// 反射获取获取对象属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="propertyName"></param>
    /// <param name="bindingAtt"></param>
    /// <returns></returns>
    public static T? PropertyValue<T>(this object instance, string propertyName,
        BindingFlags bindingAtt = BindingFlags.Public | BindingFlags.Instance)
    {
        var propertyValue = (instance.GetType().GetProperty(propertyName, bindingAtt)
            ?.GetValue(instance));
        if (propertyValue == null)
        {
            return default;
        }

        return (T)propertyValue;
    }
}