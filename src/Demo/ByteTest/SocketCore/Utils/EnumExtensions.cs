using System.ComponentModel;

namespace SocketCore.Utils;

public static class EnumExtensions
{
    public static string Description(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }
}