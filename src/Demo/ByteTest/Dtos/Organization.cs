using MessagePack;

namespace ByteTest.Dtos;

[MessagePackObject]
public class Organization
{
    [Key(0)]
    public int Id { get; set; }

    [Key(1)]
    public string[]? Tags { get; set; }

    [Key(2)]
    public List<People>? Members { get; set; }
}