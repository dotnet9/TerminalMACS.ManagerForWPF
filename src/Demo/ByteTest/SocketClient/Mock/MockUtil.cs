using Process = SocketDto.Process;

namespace SocketClient.Mock;

public static class MockUtil
{
    public const int MockCount = 1000000;
    public const int MockPageSize = 5000;

    public static List<Process> MockProcesses(int pageIndex)
    {
        var currentDataCount = GetDataCount(pageIndex, MockCount, MockPageSize);
        return ProcessReader.MockProcesses(pageIndex * MockPageSize, currentDataCount).Select(process => new Process()
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
        }).ToList();
    }


    public static int GetPageCount(int totalCount, int pageSize)
    {
        return (totalCount + pageSize - 1) / pageSize;
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