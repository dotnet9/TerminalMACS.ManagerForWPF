using ByteTest.Core.Models;
using System.Text.Json;

namespace ByteTest.Core.Helpers;

public class JsonSerializeHelper : ISerializeHelper
{
    public byte[] Serialize(Organization data)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }

    public Organization? Deserialize(byte[] buffer)
    {
        var data = JsonSerializer.Deserialize<Organization>(buffer);
        return data;
    }
}