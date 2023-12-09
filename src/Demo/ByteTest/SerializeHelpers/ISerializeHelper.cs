using ByteTest.Dtos;

namespace ByteTest.SerialHelpers;

internal interface ISerializeHelper
{
    string Name();
    byte[] Serialize(Organization data);

    Organization? Deserialize(byte[] buffer);
}