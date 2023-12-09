using MessagePack;

namespace ByteTest.Dtos;

[MessagePackObject]
public class People
{
    [Key(0)] public int Id { get; set; }

    [Key(1)] public string? Name { get; set; }

    [Key(2)] public string? Description { get; set; }

    [Key(3)] public string? Address { get; set; }

    [Key(4)] public double Value { get; set; }

    [Key(5)] public long UpdateTime { get; set; }
}