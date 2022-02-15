using System.Collections.Generic;
using TerminalMACS.Utils.NetObjectHelper;

namespace TerminalMACS.Utils.UnitTest.Models;

/// <summary>
///     三国
/// </summary>
[NetObject(Name = "ThreeCountries", Version = 1)]
public class ThreeCountries
{
    public static NetObjectAttribute CurrentObject;

    static ThreeCountries()
    {
        CurrentObject = NetObjectSerializeHelper.GetAttribute<ThreeCountries, NetObjectAttribute>(default);
    }

    /// <summary>
    ///     获取或者设置 ID
    /// </summary>
    [NetObjectProperty(ID = 1)]
    public int ID { get; set; }

    /// <summary>
    ///     获取或者设置 国名
    /// </summary>
    [NetObjectProperty(ID = 2)]
    public string Name { get; set; }

    /// <summary>
    ///     获取或者设置 皇帝
    /// </summary>
    [NetObjectProperty(ID = 3)]
    public string Emperor { get; set; }

    /// <summary>
    ///     获取或者设置 所选课程列表
    /// </summary>
    [NetObjectProperty(ID = 4)]
    public List<FamousGeneral> Courses { get; set; }

    public override string ToString()
    {
        return $"三国之一{ID}：{Name}皇帝{Emperor},有 {Courses.Count}名大将";
    }
}