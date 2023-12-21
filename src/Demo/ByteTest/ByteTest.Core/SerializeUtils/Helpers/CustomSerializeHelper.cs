using System.Reflection;

namespace ByteTest.Core.SerializeUtils.Helpers;

public class CustomSerializeHelper : ISerializeHelper
{
    private static readonly Dictionary<string, List<PropertyInfo>> ObjectPropertyInfos = new();
    private static readonly List<string> ComplexTypeNames;

    static CustomSerializeHelper()
    {
        ComplexTypeNames = new List<string>
        {
            typeof(List<>).Name,
            typeof(Dictionary<,>).Name
        };
    }

    public byte[] Serialize<T>(T data)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, SerializeConst.DefaultEncoding);

        var type = typeof(T);
        writer.Write(type.Name);
        Serialize(writer, data);

        return stream.ToArray();
    }

    public T? Deserialize<T>(byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var data = Activator.CreateInstance<T>();
        var name = reader.ReadString();
        Deserialize(reader, data);

        return data;
    }

    #region 序列化操作

    private static void Serialize<T>(BinaryWriter writer, T data)
    {
        var properties = GetProperties(data!.GetType());
        foreach (var property in properties)
        {
            Serialize(writer, data, property);
        }
    }

    private static void Serialize<T>(BinaryWriter writer, T data, PropertyInfo property)
    {
        var propertyType = property.PropertyType;
        var propertyValue = property.GetValue(data, null);
        Serialize(writer, propertyValue, propertyType);
    }

    private static void Serialize(BinaryWriter writer, object value, Type valueType)
    {
        var propertyName = valueType.Name;
        if (valueType.IsPrimitive
            || valueType.BaseType == typeof(ValueType)
            || valueType == typeof(string)
            || valueType == typeof(byte[]))
        {
            SerializeBase(writer, value, valueType);
        }
        else if (ComplexTypeNames.Contains(propertyName))
        {
            SerializeComplex(writer, value, valueType);
        }
        else
        {
            Serialize(writer, value);
        }
    }


    private static void SerializeBase(BinaryWriter writer, object value, Type valueType)
    {
        if (valueType == typeof(byte))
        {
            writer.Write(value == null ? default : byte.Parse(value.ToString()));
        }
        else if (valueType == typeof(short))
        {
            writer.Write(value == null ? default : short.Parse(value.ToString()));
        }
        else if (valueType == typeof(int))
        {
            writer.Write(value == null ? default : int.Parse(value.ToString()));
        }
        else if (valueType == typeof(long))
        {
            writer.Write(value == null ? default : long.Parse(value.ToString()));
        }
        else if (valueType == typeof(double))
        {
            writer.Write(value == null ? default : double.Parse(value.ToString()));
        }
        else if (valueType == typeof(decimal))
        {
            writer.Write(value == null ? default : decimal.Parse(value.ToString()));
        }
        else if (valueType == typeof(string))
        {
            writer.Write(value == null ? string.Empty : value.ToString());
        }
    }

    private static void SerializeComplex(BinaryWriter writer, object value, Type valueType)
    {
        var propertyName = valueType.Name;
        int count = 0;
        if (value == null)
        {
            writer.Write(count);
            return;
        }

        var genericArguments = valueType.GetGenericArguments();
        dynamic dynamicValue = value;
        count = dynamicValue.Count;
        writer.Write(count);
        if (propertyName.Equals(typeof(List<>).Name))
        {
            foreach (var item in dynamicValue)
            {
                Serialize(writer, item, genericArguments[0]);
            }
        }
        else
        {
            foreach (var item in dynamicValue)
            {
                Serialize(writer, item.Key, genericArguments[0]);
                Serialize(writer, item.Value, genericArguments[1]);
            }
        }
    }

    #endregion

    #region 反序列化操作

    private static void Deserialize<T>(BinaryReader reader, T data)
    {
        var properties = GetProperties(data!.GetType());
        foreach (var property in properties)
        {
            object value = DeserializeByType(reader, property.PropertyType);
            property.SetValue(data, value);
        }
    }

    private static object DeserializeByType(BinaryReader reader, Type propertyType)
    {
        var propertyName = propertyType.Name;
        object value;
        if (propertyType.IsPrimitive
            || propertyType.BaseType == typeof(ValueType)
            || propertyType == typeof(string)
            || propertyType == typeof(byte[]))
        {
            value = DeserializeBase(reader, propertyType);
        }
        else if (ComplexTypeNames.Contains(propertyName))
        {
            value = DeserializeComplex(reader, propertyType);
        }
        else
        {
            value = DeserializeClass(reader, propertyType);
        }

        return value;
    }

    private static object DeserializeBase(BinaryReader reader, Type propertyType)
    {
        object value;
        if (propertyType == typeof(byte))
        {
            value = reader.ReadByte();
        }
        else if (propertyType == typeof(short))
        {
            value = reader.ReadInt16();
        }
        else if (propertyType == typeof(int))
        {
            value = reader.ReadInt32();
        }
        else if (propertyType == typeof(long))
        {
            value = reader.ReadInt64();
        }
        else if (propertyType == typeof(double))
        {
            value = reader.ReadDouble();
        }
        else if (propertyType == typeof(decimal))
        {
            value = reader.ReadDecimal();
        }
        else if (propertyType == typeof(string))
        {
            value = reader.ReadString();
        }
        else
        {
            throw new Exception($"暂时未支持数据类型：{propertyType.Name}");
        }

        return value;
    }

    private static object DeserializeComplex(BinaryReader reader, Type propertyType)
    {
        var complextObj = Activator.CreateInstance(propertyType);
        var count = reader.ReadInt32();
        var addMethod = propertyType.GetMethod("Add")!;
        var genericArguments = propertyType.GetGenericArguments();
        for (int i = 0; i < count; i++)
        {
            var key = DeserializeByType(reader, genericArguments[0]);
            if (genericArguments.Length == 1)
            {
                addMethod.Invoke(complextObj, new object[] { key });
            }
            else if (genericArguments.Length == 2)
            {
                var value = DeserializeByType(reader, genericArguments[1]);
                addMethod.Invoke(complextObj, new object[] { key, value });
            }
        }

        return complextObj;
    }

    private static object DeserializeClass(BinaryReader reader, Type type)
    {
        var data = Activator.CreateInstance(type);
        Deserialize(reader, data);
        return data;
    }

    #endregion


    private static List<PropertyInfo> GetProperties(Type type)
    {
        var objectName = type.Name;
        if (ObjectPropertyInfos.TryGetValue(objectName, out List<PropertyInfo>? propertyInfos))
        {
            return propertyInfos;
        }

        propertyInfos = type.GetProperties().ToList();
        ObjectPropertyInfos[objectName] = propertyInfos;
        return propertyInfos;
    }
}