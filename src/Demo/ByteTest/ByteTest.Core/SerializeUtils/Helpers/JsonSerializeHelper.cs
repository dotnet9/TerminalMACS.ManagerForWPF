using System.Text.Json;

namespace ByteTest.Core.SerializeUtils.Helpers;

public class JsonSerializeHelper : ISerializeHelper
{
    public byte[] Serialize<T>(T data)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }

    public T? Deserialize<T>(byte[] buffer)
    {
        return JsonSerializer.Deserialize<T>(buffer);
    }
}