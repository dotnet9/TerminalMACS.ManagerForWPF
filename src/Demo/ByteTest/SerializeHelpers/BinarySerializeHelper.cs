using ByteTest.Dtos;
using ByteTest.SerialHelpers;

namespace ByteTest.SerializeHelpers;

public class BinarySerializeHelper : ISerializeHelper
{
    public string Name()
    {
        return "Binary序列化";
    }

    public byte[] Serialize(Organization data)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, ByteHelper.DefaultEncoding);
        writer.Write(data.Id);
        Write(writer, data.Tags);
        Write(writer, data.Members);

        return stream.ToArray();
    }

    public Organization Deserialize(byte[] buffer)
    {
        var data = new Organization();
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream, ByteHelper.DefaultEncoding);
        data.Id = reader.ReadInt32();
        data.Tags = ReadStringList(reader);
        data.Members = ReadPeopleList(reader);

        return data;
    }

    private static void Write(BinaryWriter writer, string[]? data)
    {
        var count = data?.Length ?? 0;
        writer.Write(count);
        if (count <= 0)
        {
            return;
        }

        foreach (var item in data!)
        {
            writer.Write(item);
        }
    }

    private static void Write(BinaryWriter writer, List<People>? data)
    {
        var count = data?.Count ?? 0;
        writer.Write(count);
        if (count > 0)
        {
            foreach (var item in data)
            {
                writer.Write(item.Id);
                writer.Write(item.Name ?? string.Empty);
                writer.Write(item.Description ?? string.Empty);
                writer.Write(item.Address ?? string.Empty);
                writer.Write(item.Value);
                writer.Write(item.UpdateTime);
            }
        }
    }

    private static string[]? ReadStringList(BinaryReader reader)
    {
        var count = reader.ReadInt32();
        if (count <= 0)
        {
            return default;
        }

        var values = new string[count];
        for (int i = 0; i < count; i++)
        {
            values[i] = reader.ReadString();
        }

        return values;
    }

    private static List<People>? ReadPeopleList(BinaryReader reader)
    {
        var count = reader.ReadInt32();
        if (count <= 0)
        {
            return default;
        }

        var values = new List<People>();
        for (int i = 0; i < count; i++)
        {
            var item = new People();
            item.Id = reader.ReadInt32();
            item.Name = reader.ReadString();
            item.Description = reader.ReadString();
            item.Address = reader.ReadString();
            item.Value = reader.ReadDouble();
            item.UpdateTime = reader.ReadInt64();

            values.Add(item);
        }

        return values;
    }
}