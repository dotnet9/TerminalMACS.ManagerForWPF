using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

[ProtoContract]
[MessagePackObject]
public class Member
{
    [ProtoMember(1)] [Key(0)] public int Id { get; set; }

    [ProtoMember(2)] [Key(1)] public string? Name { get; set; }

    [ProtoMember(3)] [Key(2)] public string? Description { get; set; }

    [ProtoMember(4)] [Key(3)] public string? Address { get; set; }

    [ProtoMember(5)] [Key(4)] public double Value { get; set; }

    [ProtoMember(6)] [Key(5)] public long UpdateTime { get; set; }
}