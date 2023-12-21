using DryIoc.ImTools;
using LoremNET;
using SocketCore.SysProcess.Models;
using SocketCore.Utils;
using Process = SocketDto.Process;

namespace SocketServer.Mock;

public static class MockUtil
{
    private static int _mockCount;
    private static List<Process>? _mockProcesses;
    private static List<ActiveProcess>? _mockUpdateProcesses;
    public const int UdpUpdateMilliseconds = 200;
    public const int UdpSendMilliseconds = 200;

    public static ResponseBaseInfo MockBase(int taskId = default)
    {
        return new ResponseBaseInfo()
        {
            TaskId = taskId,
            OperatingSystemType = "Windows 11",
            MemorySize = 48 * 1024,
            ProcessorCount = 8,
            TotalDiskSpace = 1024 + 256,
            NetworkBandwidth = 1024,
            IpAddress = "192.32.35.23",
            ServerName = "Windows server 2021",
            DataCenterLocation = "成都",
            IsRunning = (byte)((ProcessRunningStatus)Enum.Parse(typeof(ProcessRunningStatus),
                Random.Shared.Next(0, Enum.GetNames(typeof(ProcessRunningStatus)).Length).ToString())),
            LastUpdateTime = TimestampHelper.GetTimestamp()
        };
    }

    public static List<Process> MockProcesses(int totalCount, int pageSize, int pageIndex)
    {
        MockAllProcess(totalCount);
        return _mockProcesses!.Skip(pageIndex * pageSize).Take(pageSize).ToList();
    }

    public static List<Process> MockProcesses(int totalCount, int pageSize)
    {
        MockAllProcess(totalCount);
        var pageCount = GetPageCount(totalCount, pageSize);
        var pageIndex = Random.Shared.Next(0, pageCount);
        return _mockProcesses!.Skip(pageIndex * pageSize).Take(pageSize).ToList();
    }

    public static async Task MockAllProcess(int totalCount)
    {
        if (_mockCount == totalCount && _mockProcesses?.Count == totalCount &&
            _mockUpdateProcesses?.Count == totalCount)
        {
            return;
        }

        var sw = Stopwatch.StartNew();
        _mockCount = totalCount;

        _mockProcesses?.Clear();
        _mockUpdateProcesses = null;

        _mockProcesses = Enumerable.Range(0, _mockCount).Select(MockProcess).ToList();
        sw.Stop();
        Logger.Info($"模拟{_mockCount}条进程{sw.ElapsedMilliseconds}ms");
        _mockUpdateProcesses = Enumerable.Range(0, _mockCount).Select(index => new ActiveProcess() { PID = index + 1 })
            .ToList();
        MockUpdateProcess(_mockCount);
        await Task.CompletedTask;
    }

    private static Process MockProcess(int id)
    {
        return new Process()
        {
            PID = id + 1,
            Name = Lorem.Words(1, 3),
            Type = (byte)Random.Shared.Next(0, Enum.GetNames(typeof(ProcessType)).Length),
            Status = (byte)Random.Shared.Next(0, Enum.GetNames(typeof(ProcessStatus)).Length),
            Publisher = Lorem.Words(1, 3),
            CommandLine = Lorem.Words(1, 3),
            CPUUsage = Random.Shared.NextDouble(),
            MemoryUsage = Random.Shared.NextDouble(),
            DiskUsage = Random.Shared.NextDouble(),
            NetworkUsage = Random.Shared.NextDouble(),
            GPU = Random.Shared.NextDouble(),
            GPUEngine = Lorem.Words(1, 3),
            PowerUsage = (byte)Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length),
            PowerUsageTrend = (byte)Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length),
            LastUpdateTime = TimestampHelper.GetTimestamp(),
            UpdateTime = TimestampHelper.GetTimestamp()
        };
    }


    public static List<ActiveProcess> MockUpdateProcess(int totalCount, int pageSize, int pageIndex)
    {
        MockAllProcess(totalCount);
        return _mockUpdateProcesses!.Skip(pageIndex * pageSize).Take(pageSize).ToList();
    }

    public static void MockUpdateProcess(int totalCount)
    {
        MockAllProcess(totalCount);

        var cpuUsage = Random.Shared.NextDouble();
        var memoryUsage = Random.Shared.NextDouble();
        var diskUsage = Random.Shared.NextDouble();
        var networkUsage = Random.Shared.NextDouble();
        var gpu = Random.Shared.NextDouble();
        var powerUsage =
            (byte)(Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length));
        var powerUsageTrend =
            (byte)(Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length));
        var updateTime = TimestampHelper.GetTimestamp();

        _mockUpdateProcesses!.ForEach(process =>
        {
            process.CPUUsage = cpuUsage;
            process.MemoryUsage = memoryUsage;
            process.DiskUsage = diskUsage;
            process.NetworkUsage = networkUsage;
            process.GPU = gpu;
            process.PowerUsage =
                powerUsage;
            process.PowerUsageTrend =
                powerUsageTrend;
            process.UpdateTime = updateTime;
        });
    }

    public static int GetPageCount(int totalCount, int pageSize)
    {
        return (totalCount + pageSize - 1) / pageSize;
    }

    public static void MockUpdateActiveProcessPageCount(int totalCount, int packetSize, out int pageSize,
        out int pageCount)
    {
        // sizeof(int)为Processes长度点位4个字节
        pageSize = (packetSize - SerializeHelper.PacketHeadLen - sizeof(int)) /
                   ActiveProcess.ObjectSize;
        pageCount = GetPageCount(totalCount, pageSize);
    }

    public static int GetDataCount(int totalCount, int pageSize, int pageIndex)
    {
        var dataIndex = pageIndex * pageSize;
        var dataCount = totalCount - dataIndex;
        if (dataCount > pageSize)
        {
            dataCount = pageSize;
        }

        return dataCount;
    }
}