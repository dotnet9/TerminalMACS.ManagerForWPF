using SocketCore.SysProcess.Models;
using SocketCore.Utils;
using Process = SocketDto.Process;

namespace SocketServer.Mock;

public static class MockUtil
{
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
        var currentDataCount = GetDataCount(totalCount, pageSize, pageIndex);
        return ProcessReader.MockProcesses(pageIndex * pageSize, currentDataCount).Select(Convert).ToList();
    }

    public static List<Process> MockProcesses(int totalCount, int pageSize)
    {
        var uniquePointIndex = new HashSet<int>();
        while (uniquePointIndex.Count < pageSize)
        {
            var randomNumber = Random.Shared.Next(1, totalCount);
            uniquePointIndex.Add(randomNumber);
        }

        return ProcessReader.MockProcesses(uniquePointIndex).Select(Convert).ToList();
    }

    private static Process Convert(ProcessInfo process)
    {
        return new Process()
        {
            PID = process.PID,
            Name = process.Name,
            Type = (byte)process.Type,
            Status = (byte)process.Status,
            Publisher = process.Publisher,
            CommandLine = process.CommandLine,
            CPUUsage = process.CPUUsage,
            MemoryUsage = process.MemoryUsage,
            DiskUsage = process.DiskUsage,
            NetworkUsage = process.NetworkUsage,
            GPU = process.GPU,
            GPUEngine = process.GPUEngine,
            PowerUsage = (byte)process.PowerUsage,
            PowerUsageTrend = (byte)process.PowerUsageTrend
        };
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