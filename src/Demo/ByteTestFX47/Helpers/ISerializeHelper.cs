using ByteTestFX47.Dtos;

namespace ByteTestFX47.Helpers;

public interface ISerializeHelper
{
    string Name();
    byte[] Serialize(Organization data);

    Organization? Deserialize(byte[] buffer);
}