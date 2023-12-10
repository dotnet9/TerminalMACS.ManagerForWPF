using ByteTest.Core.Models;

namespace ByteTest.Core.Helpers;

public interface ISerializeHelper
{
    byte[] Serialize(Organization data);

    Organization? Deserialize(byte[] buffer);
}