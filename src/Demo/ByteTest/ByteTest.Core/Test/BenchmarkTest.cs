using BenchmarkDotNet.Attributes;
using ByteTest.Core.Helpers;
using ByteTest.Core.Models;
using MessagePack;
using System.Diagnostics;

namespace ByteTest.Core.Test;

[MemoryDiagnoser, RankColumn]
public class BenchmarkTest
{
    /// <summary>
    /// 测试数据量
    /// </summary>
    private const int MockCount = 1000000;

    private static readonly Random RandomShared = new(DateTime.Now.Millisecond);

    /// <summary>
    /// 测试数据
    /// </summary>
    private static ResponseOrganizations MockData { get; }

    static BenchmarkTest()
    {
        MockData = new ResponseOrganizations()
        {
            Organizations = Enumerable.Range(0, MockCount).Select(index => new Organization()).ToList()
        };
    }


    //[Benchmark]
    //public void JsonByteSerialize()
    //{
    //    RunSerialize(new JsonSerializeHelper());
    //}

    [Benchmark]
    public void CustomSerialize()
    {
        RunSerialize(new CustomSerializeHelper());
    }

    [Benchmark]
    public void ProtoBufSerialize()
    {
        RunSerialize(new ProtoBufSerializeHelper());
    }

    [Benchmark]
    public void MessagePackSerialize()
    {
        RunSerialize(new MessagePackSerializeHelper());
    }


    /// <summary>
    /// 简单测试
    /// </summary>
    public static void Test(List<ISerializeHelper>? moreHelpers = null)
    {
        MessagePackSerializer.DefaultOptions = MessagePack.Resolvers.ContractlessStandardResolver.Options;
        var serializeHelpers = new List<ISerializeHelper>
        {
            new CustomSerializeHelper(),
            new ProtoBufSerializeHelper(),
            new MessagePackSerializeHelper(),
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

        var buffer = helper.Serialize(MockData);

        sw.Stop();
        Log($"{helper.GetType().Name} Serialize {sw.ElapsedMilliseconds}ms {buffer.Length}byte");

        sw.Restart();

        var data = helper.Deserialize<ResponseOrganizations>(buffer);

        sw.Stop();

        Log($"{helper.GetType().Name} Deserialize {sw.ElapsedMilliseconds}ms {data?.Organizations?.Count}项");
    }

    private static void Log(string log)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}: {log}");
    }
}