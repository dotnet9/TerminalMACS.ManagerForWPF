using SocketCore.SysProcess.Models;
using SocketCore.Utils;
using System.ComponentModel.DataAnnotations;
using Process = SocketDto.Process;

namespace SocketServer.Mock;

public static class MockUtil
{
    public const int MockCount = 1000000;
    public const int MockPageSize = 5000;
    public const int UdpPacketSize = 65507;

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
            IsRunning = true,
            LastUpdateTime = TimestampHelper.GetTimestamp()
        };
    }

    public static List<Process> MockProcesses(int pageIndex)
    {
        var currentDataCount = GetDataCount(pageIndex, MockCount, MockPageSize);
        return ProcessReader.MockProcesses(pageIndex * MockPageSize, currentDataCount).Select(Convert).ToList();
    }

    public static List<Process> MockProcesses()
    {
        var uniquePointIndex = new HashSet<int>();
        while (uniquePointIndex.Count < MockPageSize)
        {
            var randomNumber = Random.Shared.Next(1, MockCount);
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

    public static void MockUpdateActiveProcessPageCount(out int pageSize, out int pageCount)
    {
        // sizeof(int)为Processes长度点位4个字节
        pageSize = (UdpPacketSize - SerializeHelper.PacketHeadLen - sizeof(int)) /
                   ActiveProcess.ObjectSize;
        pageCount = GetPageCount(MockCount, pageSize);
    }

    public static int GetDataCount(int pageIndex, int totalCount,
        int pageSize)
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