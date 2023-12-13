using ProtoBuf;

namespace ByteTest.Core.Helpers;

public class ProtoBufSerializeHelper : ISerializeHelper
{
    public byte[] Serialize<T>(T data)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, data);
        return stream.ToArray();
    }

    public T Deserialize<T>(byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        return Serializer.Deserialize<T>(stream);
    }
}