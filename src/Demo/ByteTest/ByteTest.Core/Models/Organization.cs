using ProtoBuf;

namespace ByteTest.Core.Models;

[ProtoContract]
public class Organization
{
    [ProtoMember(1)] public int Id { get; set; }

    [ProtoMember(2)] public string[]? Tags { get; set; }

    [ProtoMember(3)] public List<Member>? Members { get; set; }
}