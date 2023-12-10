using ByteTest.Core.Models;
using ProtoBuf;

namespace ByteTest.Core.Helpers;

public class ProtoBufSerializeHelper : ISerializeHelper
{
    public string Name()
    {
        return "ProtoBuf";
    }

    public byte[] Serialize(Organization data)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, data);
        return stream.ToArray();
    }

    public Organization? Deserialize(byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        return Serializer.Deserialize<Organization>(stream);
    }
}