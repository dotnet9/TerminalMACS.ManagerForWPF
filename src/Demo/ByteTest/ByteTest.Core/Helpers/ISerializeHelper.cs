namespace ByteTest.Core.Helpers;

public interface ISerializeHelper
{
    byte[] Serialize<T>(T data);

    T? Deserialize<T>(byte[] buffer);
}