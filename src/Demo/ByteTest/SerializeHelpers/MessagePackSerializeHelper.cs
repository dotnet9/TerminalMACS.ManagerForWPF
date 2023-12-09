using ByteTest.Dtos;
using ByteTest.SerialHelpers;
using MessagePack;

namespace ByteTest.SerializeHelpers;

internal class MessagePackSerializeHelper : ISerializeHelper
{
    public string Name()
    {
        return "MessagePack序列化";
    }

    public byte[] Serialize(Organization data)
    {
        var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
        return MessagePackSerializer.Serialize(data, options);
    }

    public Organization? Deserialize(byte[] buffer)
    {
        var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
        return MessagePackSerializer.Deserialize<Organization>(buffer, options);
    }
}