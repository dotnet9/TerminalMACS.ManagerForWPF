using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

/// <summary>
/// 部门
/// </summary>
[ProtoContract]
[MessagePackObject]
public class Department
{
    /// <summary>
    /// Id
    /// </summary>
    [ProtoMember(1)]
    [Key(0)]
    public int Id { get; set; }

    /// <summary>
    /// 部门编码
    /// </summary>
    [ProtoMember(2)]
    [Key(1)]
    public string? Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [ProtoMember(3)]
    [Key(2)]
    public string? Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [ProtoMember(4)]
    [Key(3)]
    public string? Description { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    [ProtoMember(5)]
    [Key(4)]
    public string? Location { get; set; }

    /// <summary>
    /// 员工数量
    /// </summary>
    [ProtoMember(6)]
    [Key(5)]
    public int EmployeeCount { get; set; }

    /// <summary>
    /// 预算
    /// </summary>
    [ProtoMember(7)]
    [Key(6)]
    public decimal Budget { get; set; }

    /// <summary>
    /// 未知
    /// </summary>
    [ProtoMember(8)]
    [Key(7)]
    public double Value { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [ProtoMember(9)]
    [Key(8)]
    public long CreateTime { get; set; }
}