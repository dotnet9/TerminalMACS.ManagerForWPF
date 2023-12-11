using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

[ProtoContract]
[MessagePackObject]
public class Organization
{
    [ProtoMember(1)] [Key(0)] public int Id { get; set; }

    [ProtoMember(2)] [Key(1)] public string[]? Tags { get; set; }

    [ProtoMember(3)] [Key(2)] public List<Member>? Members { get; set; }
}