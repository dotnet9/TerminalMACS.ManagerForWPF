using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ByteTest.Dtos;
using ByteTest.SerialHelpers;
using System.Text.Json;

namespace ByteTest.SerializeHelpers;

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