using ByteTest.Dtos;
using ByteTest.SerialHelpers;
using ProtoBuf;

namespace ByteTest.SerializeHelpers;

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