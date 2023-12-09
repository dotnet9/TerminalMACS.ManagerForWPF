using BenchmarkDotNet.Attributes;
using ByteTest.Dtos;
using ByteTest.SerialHelpers;
using ByteTest.SerializeHelpers;
using System.Diagnostics;

namespace ByteTest.Benchmark;

[MemoryDiagnoser, RankColumn]
public class BenchmarkTest
{
    /// <summary>
    /// 测试数据量
    /// </summary>
    private const int DataCount = 300000;

    private static readonly Random RandomShared = new(DateTime.Now.Millisecond);

    /// <summary>
    /// 测试数据
    /// </summary>
    private static readonly Organization TestData = new()
    {
        Id = 1,
        Tags = Enumerable.Range(0, 5).Select(index => $"测试标签{index}").ToArray(),
        Members = Enumerable.Range(0, DataCount).Select(index => new People()
        {
            Id = index,
            Name = $"测试名字{index}",
            Description = $"测试描述{RandomShared.Next(1, int.MaxValue)}",
            Address = $"测试地址{RandomShared.Next(1, int.MaxValue)}",
            Value = RandomShared.Next(1, int.MaxValue),
            UpdateTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds()
        }).ToList()
    };

    [Benchmark]
    public void BinarySerialize()
    {
        RunSerialize(new BinarySerializeHelper());
    }

    [Benchmark]
    public void CustomSerialize()
    {
        RunSerialize(new CustomSerializeHelper());
    }

    [Benchmark]
    public void JsonByteSerialize()
    {
        RunSerialize(new JsonByteSerializeHelper());
    }

    [Benchmark]
    public void MessagePackSerialize()
    {
        RunSerialize(new MessagePackSerializeHelper());
    }

    [Benchmark]
    public void ProtoBufPackSerialize()
    {
        RunSerialize(new ProtoBufSerializeHelper());
    }

    /// <summary>
    /// 简单测试
    /// </summary>
    public static void Test()
    {
        var serializeHelpers = new List<ISerializeHelper>
        {
            new BinarySerializeHelper(),
            new CustomSerializeHelper(),
            new JsonByteSerializeHelper(),
            new MessagePackSerializeHelper(),
            new ProtoBufSerializeHelper()
        };
        serializeHelpers.ForEach(RunSerialize);
    }


    private static void RunSerialize(ISerializeHelper helper)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var buffer = helper.Serialize(TestData);

        sw.Stop();
        Log($"{helper.Name()} Serialize：{sw.ElapsedMilliseconds}ms，{buffer.Length}byte");

        sw.Restart();

        var data = helper.Deserialize(buffer);

        sw.Stop();

        Log($"{helper.Name()} Deserialize：{sw.ElapsedMilliseconds}ms，{data?.Members?.Count}项");
    }

    private static void Log(string log)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}：{log}");
    }
}