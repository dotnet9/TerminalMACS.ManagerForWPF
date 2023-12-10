using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

[MessagePackObject]
[ProtoContract]
public class Member
{
    [Key(0)] [ProtoMember(1)] public int Id { get; set; }

    [Key(1)] [ProtoMember(2)] public string? Name { get; set; }

    [Key(2)] [ProtoMember(3)] public string? Description { get; set; }

    [Key(3)] [ProtoMember(4)] public string? Address { get; set; }

    [Key(4)] [ProtoMember(5)] public double Value { get; set; }

    [Key(5)] [ProtoMember(6)] public long UpdateTime { get; set; }
}