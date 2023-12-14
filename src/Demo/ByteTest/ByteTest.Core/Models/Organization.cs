using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

/// <summary>
/// 组织
/// </summary>
[ProtoContract]
[MessagePackObject]
public class Organization
{
    /// <summary>
    /// Id
    /// </summary>
    [ProtoMember(1)]
    [Key(0)]
    public int Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [ProtoMember(2)]
    [Key(1)]
    public string? Name { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    [ProtoMember(3)]
    [Key(2)]
    public List<string>? Tags { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [ProtoMember(4)]
    [Key(3)]
    public string? Address { get; set; }

    /// <summary>
    /// 员工总数
    /// </summary>
    [ProtoMember(5)]
    [Key(4)]
    public int EmployeeCount { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [ProtoMember(6)]
    [Key(5)]
    public List<Department>? Departments { get; set; }

    /// <summary>
    /// 年度预算
    /// </summary>
    [ProtoMember(7)]
    [Key(6)]
    public decimal AnnualBudget { get; set; }

    /// <summary>
    /// 成立日期，时间戳，单位ms
    /// </summary>
    [ProtoMember(8)]
    [Key(7)]
    public long FoundationDate { get; set; }
}