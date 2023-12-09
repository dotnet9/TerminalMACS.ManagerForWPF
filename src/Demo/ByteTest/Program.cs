using ByteTest.Dtos;
using ByteTest.SerialHelpers;
using ByteTest.SerializeHelpers;
using System.Diagnostics;

int dataCount = 3000000;
Log($@"测试数据包序列化\反序列化{dataCount}条数据");

// 1、准备测试数据

var data = new Organization()
{
    Id = 1,
    Tags = Enumerable.Range(0, 5).Select(index => $"测试标签{index}").ToArray(),
    Members = Enumerable.Range(0, dataCount).Select(index => new People()
    {
        Id = index,
        Name = $"测试名字{index}",
        Description = $"测试描述{Random.Shared.Next(1, int.MaxValue)}",
        Address = $"测试地址{Random.Shared.Next(1, int.MaxValue)}",
        Value = Random.Shared.Next(1, int.MaxValue),
        UpdateTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds()
    }).ToList()
};

// 2、测试序列化速度
var serializeHelper = new List<ISerializeHelper>
{
    new JsonSerializeHelper(), new CustomSerializeHelper(), new BinarySerializeHelper(),
    new MessagePackSerializeHelper()
};
Parallel.ForEach(serializeHelper, helper =>
{
    var buffer = CountSerialize(helper, data);
    CountDeserialize(helper, buffer);
});
Console.ReadKey();

byte[] CountSerialize(ISerializeHelper helper, Organization data)
{
    Stopwatch sw = Stopwatch.StartNew();

    var buffer = helper.Serialize(data);

    sw.Stop();

    Log($"{helper.Name()} 序列化结束，用时{sw.ElapsedMilliseconds}ms，共{buffer.Length}字节");
    return buffer;
}

void CountDeserialize(ISerializeHelper helper, byte[] buffer)
{
    Stopwatch sw = Stopwatch.StartNew();

    var data = helper.Deserialize(buffer);

    sw.Stop();

    Log($"{helper.Name()} 反序列化结束，用时{sw.ElapsedMilliseconds}ms，共{data?.Members?.Count}个成员");
}

void Log(string log)
{
    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}：{log}");
}