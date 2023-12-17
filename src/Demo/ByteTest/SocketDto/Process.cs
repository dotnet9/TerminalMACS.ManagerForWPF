namespace SocketDto;

/// <summary>
/// 操作系统进程信息
/// </summary>
[MessagePackObject]
public record Process
{
    /// <summary>
    /// 进程ID
    /// </summary>
    [Key(0)]
    public int PID { get; set; }

    /// <summary>
    /// 进程名称
    /// </summary>
    [Key(1)]
    public string? Name { get; set; }

    /// <summary>
    /// 进程类型
    /// </summary>
    [Key(2)]
    public byte Type { get; set; }

    /// <summary>
    /// 进程状态
    /// </summary>
    [Key(3)]
    public byte Status { get; set; }

    /// <summary>
    /// 发布者
    /// </summary>
    [Key(4)]
    public string? Publisher { get; set; }

    /// <summary>
    /// 命令行
    /// </summary>
    [Key(5)]
    public string? CommandLine { get; set; }

    /// <summary>
    /// CPU使用率
    /// </summary>
    [Key(6)]
    public double CPUUsage { get; set; }

    /// <summary>
    /// 内存使用大小
    /// </summary>
    [Key(7)]
    public double MemoryUsage { get; set; }

    /// <summary>
    /// 磁盘使用大小
    /// </summary>
    [Key(8)]
    public double DiskUsage { get; set; }

    /// <summary>
    /// 网络使用值
    /// </summary>
    [Key(9)]
    public double NetworkUsage { get; set; }

    /// <summary>
    /// GPU
    /// </summary>
    [Key(10)]
    public double GPU { get; set; }

    /// <summary>
    /// GPU引擎
    /// </summary>
    [Key(11)]
    public string? GPUEngine { get; set; }

    /// <summary>
    /// 电源使用情况
    /// </summary>
    [Key(12)]
    public byte PowerUsage { get; set; }

    /// <summary>
    /// 电源使用情况趋势
    /// </summary>
    [Key(13)]
    public byte PowerUsageTrend { get; set; }
}