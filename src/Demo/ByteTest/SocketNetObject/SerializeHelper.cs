using MessagePack;
using System.Text;
using SocketNetObject.Models;

namespace SocketNetObject;

/// <summary>
/// 网络对象序列化辅助类
/// </summary>
public static class SerializeHelper
{
    static readonly MessagePackSerializerOptions Options =
        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    public static Encoding DefaultEncoding { get; } = Encoding.UTF8;
    public const int PacketHeadLen = 14; //数据包包头大小

    public static byte[] Serialize<T>(this T data, long systemId)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var netObjectInfo = GetNetObjectHeadAttribute(data.GetType());
        dynamic tempObject = data; // TODO，这里需要用dynamic接一个待序列化对象，不然MessagePack会报错
        var bodyBuffer = MessagePackSerializer.Serialize(tempObject, Options);
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, DefaultEncoding);
        writer.Write(PacketHeadLen + bodyBuffer.Length);
        writer.Write(systemId);
        writer.Write(netObjectInfo.Id);
        writer.Write(netObjectInfo.Version);
        writer.Write(bodyBuffer);

        return stream.ToArray();
    }

    public static T Deserialize<T>(byte[] buffer)
    {
        var bodyBufferLen = buffer.Length - PacketHeadLen;
        using var stream = new MemoryStream(buffer, PacketHeadLen, bodyBufferLen);
        using var reader = new BinaryReader(stream);
        var bodyBuffer = reader.ReadBytes(bodyBufferLen);
        return MessagePackSerializer.Deserialize<T>(bodyBuffer, Options);
    }

    public static NetObjectHeadAttribute GetNetObjectHeadAttribute(Type netObjectType)
    {
        var attributeType = typeof(NetObjectHeadAttribute);
        var attributes = netObjectType.GetCustomAttributes(attributeType, false);
        if (attributes.Length < 1 || attributes[0] is not NetObjectHeadAttribute netObjectHeadAttribute)
        {
            throw new Exception($"{netObjectType.Name}未添加特性标注{attributeType.Name}");
        }

        return netObjectHeadAttribute;
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
        var netObjectAttribute = GetNetObjectHeadAttribute(typeof(T));
        return netObjectAttribute.Id == netObjectHeadInfo.ObjectId &&
               netObjectAttribute.Version == netObjectHeadInfo.ObjectVersion;
    }
}