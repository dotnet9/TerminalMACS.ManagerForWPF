using BenchmarkDotNet.Attributes;
using ByteTest.Core.Helpers;
using ByteTest.Core.Models;
using System.Diagnostics;

namespace ByteTest.Core.Test;

[MemoryDiagnoser, RankColumn]
public class BenchmarkTest
{
    /// <summary>
    /// 测试数据量
    /// </summary>
    private const int DataCount = 1000000;

    private static readonly Random RandomShared = new(DateTime.Now.Millisecond);

    /// <summary>
    /// 测试数据
    /// </summary>
    private static readonly Organization TestData = new()
    {
        Id = 1,
        Tags = Enumerable.Range(0, 5).Select(index => $"测试标签{index}").ToArray(),
        Members = Enumerable.Range(0, DataCount).Select(index => new Member()
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
        RunSerialize(new JsonSerializeHelper());
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
    public static void Test(List<ISerializeHelper>? moreHelpers = null)
    {
        var serializeHelpers = new List<ISerializeHelper>
        {
            new JsonSerializeHelper(),
            new CustomSerializeHelper(),
            new BinarySerializeHelper(),
            new MessagePackSerializeHelper(),
            new ProtoBufSerializeHelper(),
        };
        if (moreHelpers?.Count() > 0)
        {
            serializeHelpers.AddRange(moreHelpers);
        }

        serializeHelpers.ForEach(RunSerialize);
    }


    private static void RunSerialize(ISerializeHelper helper)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var buffer = helper.Serialize(TestData);

        sw.Stop();
        Log($"{helper.GetType().Name} Serialize {sw.ElapsedMilliseconds}ms {buffer.Length}byte");

        sw.Restart();

        var data = helper.Deserialize(buffer);

        sw.Stop();

        Log($"{helper.GetType().Name} Deserialize {sw.ElapsedMilliseconds}ms {data?.Members?.Count}项");
    }

    private static void Log(string log)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}: {log}");
    }
}