using ProtoBuf;

namespace ByteTest.Core.Models;

[ProtoContract]
public class Member
{
    [ProtoMember(1)] public int Id { get; set; }

    [ProtoMember(2)] public string? Name { get; set; }

    [ProtoMember(3)] public string? Description { get; set; }

    [ProtoMember(4)] public string? Address { get; set; }

    [ProtoMember(5)] public double Value { get; set; }

    [ProtoMember(6)] public long UpdateTime { get; set; }
}