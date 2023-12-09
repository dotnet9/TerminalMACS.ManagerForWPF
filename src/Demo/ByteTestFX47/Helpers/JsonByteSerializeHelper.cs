using System.Text.Json;
using ByteTestFX47.Dtos;

namespace ByteTestFX47.Helpers;

public class JsonByteSerializeHelper : ISerializeHelper
{
    public string Name()
    {
        return "Json";
    }

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