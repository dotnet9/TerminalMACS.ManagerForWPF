using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

[MessagePackObject]
[ProtoContract]
public class Organization
{
    [Key(0)] [ProtoMember(1)] public int Id { get; set; }

    [Key(1)] [ProtoMember(2)] public string[]? Tags { get; set; }

    [Key(2)] [ProtoMember(3)] public List<Member>? Members { get; set; }
}