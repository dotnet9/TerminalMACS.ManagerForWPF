using ByteTest.Dtos;
using ByteTest.SerialHelpers;
using System.Text;
using System.Text.Json;

namespace ByteTest.SerializeHelpers;

internal class JsonSerializeHelper : ISerializeHelper
{
    public string Name()
    {
        return "JSON序列化";
    }

    public byte[] Serialize(Organization data)
    {
        var jsonStr = JsonSerializer.Serialize(data);
        var buffer = Encoding.UTF8.GetBytes(jsonStr);
        return buffer;
    }

    public Organization? Deserialize(byte[] buffer)
    {
        var data = JsonSerializer.Deserialize<Organization>(buffer);
        return data;
    }
}