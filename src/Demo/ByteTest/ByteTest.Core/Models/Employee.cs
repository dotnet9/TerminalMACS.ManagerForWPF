using MessagePack;
using ProtoBuf;

namespace ByteTest.Core.Models;

/// <summary>
/// 员工
/// </summary>
[ProtoContract]
[MessagePackObject]
public class Employee
{
    /// <summary>
    /// Id
    /// </summary>
    [ProtoMember(1)]
    [Key(0)]
    public int Id { get; set; }


    /// <summary>
    /// 编码
    /// </summary>
    [ProtoMember(2)]
    [Key(1)]
    public string? Code { get; set; }

    /// <summary>
    /// 名
    /// </summary>
    [ProtoMember(3)]
    [Key(2)]
    public string? FirstName { get; set; }

    /// <summary>
    /// 姓
    /// </summary>
    [ProtoMember(4)]
    [Key(3)]
    public string? LastName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [ProtoMember(5)]
    [Key(4)]
    public string? NickName { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [ProtoMember(6)]
    [Key(5)]
    public long BirthDate { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [ProtoMember(7)]
    [Key(6)]
    public string? Description { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [ProtoMember(8)]
    [Key(7)]
    public string? Address { get; set; }

    /// <summary>
    /// 邮件
    /// </summary>
    [ProtoMember(9)]
    [Key(8)]
    public string? Email { get; set; }

    /// <summary>
    /// 电话号码
    /// </summary>
    [ProtoMember(10)]
    [Key(9)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 工资
    /// </summary>
    [ProtoMember(11)]
    [Key(10)]
    public decimal Salary { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [ProtoMember(12)]
    [Key(11)]
    public decimal DepartmentId { get; set; }

    /// <summary>
    /// 入职时间
    /// </summary>
    [ProtoMember(13)]
    [Key(12)]
    public long EntryTime { get; set; }
}