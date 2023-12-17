using System.Windows.Media.Animation;
using SocketCore.SysProcess.Models;
using SocketCore.Utils;
using Process = SocketDto.Process;

namespace SocketClient.Models;

/// <summary>
/// 操作系统进程信息
/// </summary>
public class ProcessItem : BindableBase
{
    /// <summary>
    /// 进程ID
    /// </summary>
    public int PID { get; set; }

    private string? _name;

    /// <summary>
    /// 进程名称
    /// </summary>
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string? _type;

    /// <summary>
    /// 进程类型
    /// </summary>
    public string? Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private string? _status;

    /// <summary>
    /// 进程状态
    /// </summary>
    public string? Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string? _publisher;

    /// <summary>
    /// 发布者
    /// </summary>
    public string? Publisher
    {
        get => _publisher;
        set => SetProperty(ref _publisher, value);
    }

    private string? _commandLine;

    /// <summary>
    /// 命令行
    /// </summary>
    public string? CommandLine
    {
        get => _commandLine;
        set => SetProperty(ref _commandLine, value);
    }

    private string? _cpuUsage;

    /// <summary>
    /// CPU使用率
    /// </summary>
    public string? CPUUsage
    {
        get => _cpuUsage;
        set => SetProperty(ref _cpuUsage, value);
    }

    private string? _memoryUsage;

    /// <summary>
    /// 内存使用大小
    /// </summary>
    public string? MemoryUsage
    {
        get => _memoryUsage;
        set => SetProperty(ref _memoryUsage, value);
    }

    private string? _diskUsage;

    /// <summary>
    /// 磁盘使用大小
    /// </summary>
    public string? DiskUsage
    {
        get => _diskUsage;
        set => SetProperty(ref _diskUsage, value);
    }

    private string? _networkUsage;

    /// <summary>
    /// 网络使用值
    /// </summary>
    public string? NetworkUsage
    {
        get => _networkUsage;
        set => SetProperty(ref _networkUsage, value);
    }

    private string? _gpu;

    /// <summary>
    /// GPU
    /// </summary>
    public string? GPU
    {
        get => _gpu;
        set => SetProperty(ref _gpu, value);
    }

    private string? _gpuEngine;

    /// <summary>
    /// GPU引擎
    /// </summary>
    public string? GPUEngine
    {
        get => _gpuEngine;
        set => SetProperty(ref _gpuEngine, value);
    }

    private string? _powerUsage;

    /// <summary>
    /// 电源使用情况
    /// </summary>
    public string? PowerUsage
    {
        get => _powerUsage;
        set => SetProperty(ref _powerUsage, value);
    }

    private string? _powerUsageTrend;

    /// <summary>
    /// 电源使用情况趋势
    /// </summary>
    public string? PowerUsageTrend
    {
        get => _powerUsageTrend;
        set => SetProperty(ref _powerUsageTrend, value);
    }

    public ProcessItem()
    {
    }

    public ProcessItem(Process process)
    {
        Update(process);
    }

    public void Update(Process process)
    {
        PID = process.PID;
        Name = process.Name;
        Type = ((ProcessType)Enum.Parse(typeof(ProcessType), process.Type.ToString())).Description();
        Status = ((ProcessStatus)Enum.Parse(typeof(ProcessStatus), process.Status.ToString())).Description();
        Publisher = process.Publisher;
        CommandLine = process.CommandLine;
        CPUUsage = process.CPUUsage.ToString("P1");
        MemoryUsage = process.MemoryUsage.ToString("P1");
        DiskUsage = $"{process.DiskUsage:P1} MB/秒";
        NetworkUsage = $"{process.DiskUsage:P1} Mbps";
        GPU = process.GPU.ToString("P1");
        GPUEngine = process.GPUEngine;
        PowerUsage = ((ProcessPowerUsage)Enum.Parse(typeof(ProcessPowerUsage), process.PowerUsage.ToString()))
            .Description();
        PowerUsageTrend = ((ProcessPowerUsage)Enum.Parse(typeof(ProcessPowerUsage), process.PowerUsageTrend.ToString()))
            .Description();
    }
}