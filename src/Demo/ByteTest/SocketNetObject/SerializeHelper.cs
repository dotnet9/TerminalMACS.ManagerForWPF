using SocketNetObject.Models;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace SocketNetObject;

public static class SerializeHelper
{
    private static readonly ConcurrentDictionary<string, List<PropertyInfo>> ObjectPropertyInfos = new();

    private static readonly List<string> ComplexTypeNames =
    [
        typeof(List<>).Name,
        typeof(Dictionary<,>).Name
    ];

    public static Encoding DefaultEncoding = Encoding.UTF8;
    public const int PacketHeadLen = 14; //数据包包头大小

    #region 序列化操作

    public static byte[] Serialize<T>(this T data, long systemId) where T : INetObject
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var netObjectInfo = GetNetObjectHead(data.GetType());
        var bodyBuffer = SerializeObject(data);
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, DefaultEncoding);

        writer.Write(PacketHeadLen + bodyBuffer.Length);
        writer.Write(systemId);
        writer.Write(netObjectInfo.Id);
        writer.Write(netObjectInfo.Version);
        writer.Write(bodyBuffer);

        return stream.ToArray();
    }

    private static byte[] SerializeObject<T>(T data)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, DefaultEncoding);
        SerializeProperties(writer, data);
        return stream.ToArray();
    }

    private static void SerializeProperties<T>(BinaryWriter writer, T data)
    {
        var properties = GetProperties(data!.GetType());
        foreach (var property in properties)
        {
            SerializeProperty(writer, data, property);
        }
    }

    private static void SerializeProperty<T>(BinaryWriter writer, T data, PropertyInfo property)
    {
        var propertyType = property.PropertyType;
        var propertyValue = property.GetValue(data)!;
        SerializeValue(writer, propertyValue, propertyType);
    }

    private static void SerializeValue(BinaryWriter writer, object value, Type valueType)
    {
        if (valueType.IsPrimitive || valueType == typeof(string) || valueType == typeof(byte[]))
        {
            SerializeBaseValue(writer, value, valueType);
        }
        else if (ComplexTypeNames.Contains(valueType.Name))
        {
            SerializeComplexValue(writer, value, valueType);
        }
        else
        {
            SerializeProperties(writer, value);
        }
    }

    private static void SerializeBaseValue(BinaryWriter writer, object? value, Type valueType)
    {
        if (valueType == typeof(byte))
        {
            writer.Write(value == null ? default : byte.Parse(value.ToString()));
        }
        else if (valueType == typeof(byte[]))
        {
            if (value is not byte[] buffer)
            {
                writer.Write(0);
            }
            else
            {
                writer.Write(buffer.Length);
                writer.Write(buffer);
            }
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

    private static void SerializeComplexValue(BinaryWriter writer, object value, Type valueType)
    {
        int count = 0;
        if (value == null)
        {
            writer.Write(count);
            return;
        }

        dynamic dynamicValue = value;
        count = dynamicValue.Count;
        writer.Write(count);

        var genericArguments = valueType.GetGenericArguments();
        if (valueType.Name.Equals(typeof(List<>).Name))
        {
            foreach (var item in dynamicValue)
            {
                SerializeValue(writer, item, genericArguments[0]);
            }
        }
        else
        {
            foreach (var item in dynamicValue)
            {
                SerializeValue(writer, item.Key, genericArguments[0]);
                SerializeValue(writer, item.Value, genericArguments[1]);
            }
        }
    }

    #endregion

    #region 反序列化操作

    public static T Deserialize<T>(this byte[] buffer) where T : new()
    {
        var bodyBufferLen = buffer.Length - PacketHeadLen;
        using var stream = new MemoryStream(buffer, PacketHeadLen, bodyBufferLen);
        using var reader = new BinaryReader(stream);
        var data = new T();
        DeserializeProperties(reader, data);
        return data;
    }

    private static void DeserializeProperties<T>(BinaryReader reader, T data)
    {
        var properties = GetProperties(data!.GetType());
        foreach (var property in properties)
        {
            object value = DeserializeValue(reader, property.PropertyType);
            property.SetValue(data, value);
        }
    }

    private static object DeserializeValue(BinaryReader reader, Type propertyType)
    {
        object value;
        if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(byte[]))
        {
            value = DeserializeBaseValue(reader, propertyType);
        }
        else if (ComplexTypeNames.Contains(propertyType.Name))
        {
            value = DeserializeComplexValue(reader, propertyType);
        }
        else
        {
            value = DeserializeObject(reader, propertyType);
        }

        return value;
    }

    private static object DeserializeBaseValue(BinaryReader reader, Type propertyType)
    {
        if (propertyType == typeof(byte))
        {
            return reader.ReadByte();
        }

        if (propertyType == typeof(byte[]))
        {
            return reader.ReadBytes(reader.ReadInt32());
        }

        if (propertyType == typeof(short))
        {
            return reader.ReadInt16();
        }

        if (propertyType == typeof(int))
        {
            return reader.ReadInt32();
        }

        if (propertyType == typeof(long))
        {
            return reader.ReadInt64();
        }

        if (propertyType == typeof(double))
        {
            return reader.ReadDouble();
        }

        if (propertyType == typeof(decimal))
        {
            return reader.ReadDecimal();
        }

        if (propertyType == typeof(string))
        {
            return reader.ReadString();
        }

        throw new Exception($"Unsupported data type: {propertyType.Name}");
    }

    private static object DeserializeComplexValue(BinaryReader reader, Type propertyType)
    {
        var count = reader.ReadInt32();
        var genericArguments = propertyType.GetGenericArguments();
        dynamic complexObj = Activator.CreateInstance(propertyType)!;
        var addMethod = propertyType.GetMethod("Add")!;

        for (int i = 0; i < count; i++)
        {
            var key = DeserializeValue(reader, genericArguments[0]);
            if (genericArguments.Length == 1)
            {
                addMethod.Invoke(complexObj, new[] { key });
            }
            else if (genericArguments.Length == 2)
            {
                var value = DeserializeValue(reader, genericArguments[1]);
                addMethod.Invoke(complexObj, new[] { key, value });
            }
        }

        return complexObj;
    }

    private static object DeserializeObject(BinaryReader reader, Type type)
    {
        var data = Activator.CreateInstance(type);
        DeserializeProperties(reader, data);
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

    public static NetObjectHeadAttribute GetNetObjectHead(this Type netObjectType)
    {
        var attribute = netObjectType.GetCustomAttribute<NetObjectHeadAttribute>();
        return attribute ?? throw new Exception(
            $"{netObjectType.Name} has not been marked with the attribute {nameof(NetObjectHeadAttribute)}");
    }

    public static bool ReadHead(byte[] buffer, ref int readIndex, out NetObjectHeadInfo? netObjectHeadInfo)
    {
        netObjectHeadInfo = null;
        if (buffer.Length < (readIndex + PacketHeadLen))
        {
            return false;
        }

        netObjectHeadInfo = new NetObjectHeadInfo();

        netObjectHeadInfo.BufferLen = BitConverter.ToInt32(buffer, readIndex);
        readIndex += sizeof(int);

        netObjectHeadInfo.SystemId = BitConverter.ToInt64(buffer, readIndex);
        readIndex += sizeof(long);

        netObjectHeadInfo.ObjectId = buffer[readIndex];
        readIndex += sizeof(byte);

        netObjectHeadInfo.ObjectVersion = buffer[readIndex];
        readIndex += sizeof(byte);

        return true;
    }

    public static bool IsNetObject<T>(this NetObjectHeadInfo netObjectHeadInfo)
    {
        var netObjectAttribute = GetNetObjectHead(typeof(T));
        return netObjectAttribute.Id == netObjectHeadInfo.ObjectId &&
               netObjectAttribute.Version == netObjectHeadInfo.ObjectVersion;
    }
}