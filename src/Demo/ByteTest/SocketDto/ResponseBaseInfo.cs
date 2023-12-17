namespace SocketDto;

/// <summary>
/// 响应基本信息
/// </summary>
[NetObjectHead(2, 1)]
[MessagePackObject]
public class ResponseBaseInfo : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    [Key(0)]
    public int TaskId { get; set; }

    /// <summary>
    /// 服务器操作系统类型
    /// </summary>
    [Key(1)]
    public string? OperatingSystemType { get; set; }

    /// <summary>
    /// 系统内存大小（单位MB）
    /// </summary>
    [Key(2)]
    public int MemorySize { get; set; }

    /// <summary>
    /// 处理器个数
    /// </summary>
    [Key(3)]
    public int ProcessorCount { get; set; }

    /// <summary>
    /// 硬盘总容量（单位GB）
    /// </summary>
    [Key(4)]
    public int TotalDiskSpace { get; set; }

    /// <summary>
    /// 网络带宽（单位Mbps）
    /// </summary>
    [Key(5)]
    public int NetworkBandwidth { get; set; }

    /// <summary>
    /// 服务器IP地址
    /// </summary>
    [Key(6)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// 服务器名称
    /// </summary>
    [Key(7)]
    public string? ServerName { get; set; }

    /// <summary>
    /// 数据中心位置
    /// </summary>
    [Key(8)]
    public string? DataCenterLocation { get; set; }

    /// <summary>
    /// 运行状态
    /// </summary>
    [Key(9)]
    public bool IsRunning { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    [Key(10)]
    public long LastUpdateTime { get; set; }
}