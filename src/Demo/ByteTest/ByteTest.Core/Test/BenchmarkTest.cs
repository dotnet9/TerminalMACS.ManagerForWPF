using BenchmarkDotNet.Attributes;
using ByteTest.Core.Helpers;
using ByteTest.Core.Models;
using MessagePack;
using System.Diagnostics;
using ByteTest.Core.SerializeUtils;

namespace ByteTest.Core.Test;

[MemoryDiagnoser, RankColumn]
public class BenchmarkTest
{
    /// <summary>
    /// 测试数据量
    /// </summary>
    private const int MockCount = 100;

    private static readonly Random RandomShared = new(DateTime.Now.Millisecond);

    /// <summary>
    /// 测试数据
    /// </summary>
    private static ResponseOrganizations MockData { get; }

    static BenchmarkTest()
    {
        MockData = new ResponseOrganizations()
        {
            Organizations = Enumerable.Range(0, MockCount).Select(orgIndex => new Organization()
            {
                Id = orgIndex,
                Name = $"Name{orgIndex}",
                Tags = Enumerable.Range(RandomShared.Next(0, 5), RandomShared.Next(10, 15))
                    .Select(tagIndex => $"标签{tagIndex}")
                    .ToList(),
                Address = $"地址{orgIndex}",
                EmployeeCount = RandomShared.Next(10, 1000),
                Departments = Enumerable.Range(2, 10).Select(i => new Department()
                {
                    Id = i,
                    Code = $"D{i}",
                    Name = $"部门{i}",
                    Description = $"描述{i}",
                    Location = $"位置{i}",
                    EmployeeCount = RandomShared.Next(5, 100),
                    Employees = Enumerable.Range(10, 100).Select(empIndex => new Employee()
                    {
                        Id = empIndex,
                        Code = $"E{empIndex}",
                        FirstName = $"名{empIndex}",
                        LastName = $"姓{empIndex}",
                        NickName = $"昵称{empIndex}",
                        BirthDate = TimestampHelper.GetTimestamp(
                            DateTime.Now.AddMilliseconds(-1 * RandomShared.Next(500000, 500000000))),
                        Description = $"描述{empIndex}",
                        Address = $"地址{empIndex}",
                        Email = $"邮件{empIndex}@dotnet9.com",
                        PhoneNumber = RandomShared.Next(1000000, 999999999).ToString(),
                        Salary = RandomShared.Next(2000, 100000),
                        DepartmentId = i,
                        EntryTime = TimestampHelper.GetTimestamp(
                            DateTime.Now.AddMilliseconds(-1 * RandomShared.Next(500000, 500000000)))
                    }).ToList(),
                    Budget = RandomShared.Next(2000, 100000) + (decimal)RandomShared.NextDouble(),
                    Value = RandomShared.NextDouble(),
                    CreateTime =
                        TimestampHelper.GetTimestamp(
                            DateTime.Now.AddMilliseconds(-1 * RandomShared.Next(500000, 500000000)))
                }).ToList(),
                AnnualBudget = RandomShared.Next(20000, 1000000) + (decimal)RandomShared.NextDouble(),
                FoundationDate =
                    TimestampHelper.GetTimestamp(
                        DateTime.Now.AddMilliseconds(-1 * RandomShared.Next(500000, 500000000)))
            }).ToList()
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