using ByteTest.Core.Models;
using MessagePack;

namespace ByteTest.Core.Helpers;

public class MessagePackSerializeHelper : ISerializeHelper
{
    // 这种方式需要在类和字段上添加特性，可加压缩
    //readonly MessagePackSerializerOptions _options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    // 这种方式不加特性也可压缩
    readonly MessagePackSerializerOptions _options =
        MessagePack.Resolvers.ContractlessStandardResolver.Options.WithCompression(MessagePackCompression
            .Lz4BlockArray);

    public byte[] Serialize(Organization data)
    {
        return MessagePackSerializer.Serialize(data, _options);
    }

    public Organization? Deserialize(byte[] buffer)
    {
        return MessagePackSerializer.Deserialize<Organization>(buffer, _options);
    }
}