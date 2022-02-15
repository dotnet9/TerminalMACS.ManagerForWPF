using System.Collections.Generic;
using TerminalMACS.Utils.NetObjectHelper;
using TerminalMACS.Utils.UnitTest.Models;
using Xunit;
using Xunit.Abstractions;

namespace TerminalMACS.Utils.UnitTest;

public class ArrayHelperUnitTest
{
    private readonly ITestOutputHelper _output;

    public ArrayHelperUnitTest(ITestOutputHelper testOutput)
    {
        _output = testOutput;
    }

    [Fact]
    public void Test1()
    {
        // Arrange
        var shuKingdom = new ThreeCountries
        {
            ID = 1,
            Name = "蜀国",
            Emperor = "刘备",
            Courses = new List<FamousGeneral>
            {
                new() { ID = 1, Name = "张飞", Memo = "三板斧" },
                new() { ID = 2, Name = "关羽", Memo = "青龙偃月刀" },
                new() { ID = 3, Name = "赵云", Memo = "很猛的" },
                new() { ID = 3, Name = "马超", Memo = "强" },
                new() { ID = 3, Name = "黄忠", Memo = "老当益壮" }
            }
        };
        _output.WriteLine($"测试对象===={shuKingdom}");

        // act
        var arrs = NetObjectSerializeHelper.Serialize(shuKingdom);
        _output.WriteLine($"序列化后字节长度：===={arrs.Length}");

        var student2 = NetObjectSerializeHelper.Deserialize<ThreeCountries>(arrs);
        _output.WriteLine($"反序列化后对象===={student2}");

        // Assert
        Assert.Equal(shuKingdom.ID, student2.ID);
        Assert.Equal(shuKingdom.Name, student2.Name);
        _output.WriteLine("序列化、反序列化测试通过");
    }
}