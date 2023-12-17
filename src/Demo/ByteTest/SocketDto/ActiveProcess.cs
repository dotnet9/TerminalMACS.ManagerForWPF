namespace SocketDto;

/// <summary>
/// 操作系统进程信息
/// </summary>
public class ActiveProcess
{
    public const int ObjectSize = 46;

    /// <summary>
    /// 进程ID
    /// </summary>
    [Key(0)]
    public int PID { get; set; }

    /// <summary>
    /// CPU使用率
    /// </summary>
    [Key(1)]
    public double CPUUsage { get; set; }

    /// <summary>
    /// 内存使用大小
    /// </summary>
    [Key(2)]
    public double MemoryUsage { get; set; }

    /// <summary>
    /// 磁盘使用大小
    /// </summary>
    [Key(3)]
    public double DiskUsage { get; set; }

    /// <summary>
    /// 网络使用值
    /// </summary>
    [Key(4)]
    public double NetworkUsage { get; set; }

    /// <summary>
    /// GPU
    /// </summary>
    [Key(5)]
    public double GPU { get; set; }

    /// <summary>
    /// 电源使用情况
    /// </summary>
    [Key(6)]
    public byte PowerUsage { get; set; }

    /// <summary>
    /// 电源使用情况趋势
    /// </summary>
    [Key(7)]
    public byte PowerUsageTrend { get; set; }
}