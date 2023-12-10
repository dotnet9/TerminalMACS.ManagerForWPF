using ByteTest.Core.Models;

namespace ByteTest.Core.Helpers;

public interface ISerializeHelper
{
    string Name();
    byte[] Serialize(Organization data);

    Organization? Deserialize(byte[] buffer);
}