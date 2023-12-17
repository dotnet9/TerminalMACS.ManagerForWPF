using LoremNET;
using SocketCore.SysProcess.Models;

namespace SocketCore.SysProcess;

public static class ProcessReader
{
    public static List<ProcessInfo> MockProcesses(int start, int count)
    {
        return Enumerable.Range(start, count).Select(index => new ProcessInfo()
        {
            PID = index,
            Name = Lorem.Words(1, 3),
            Type = (ProcessType)Enum.Parse(typeof(ProcessType),
                Random.Shared.Next(0, Enum.GetNames(typeof(ProcessType)).Length).ToString()),
            Status = (ProcessStatus)Enum.Parse(typeof(ProcessStatus),
                Random.Shared.Next(0, Enum.GetNames(typeof(ProcessStatus)).Length).ToString()),
            Publisher = Lorem.Words(1, 3),
            CommandLine = Lorem.Words(1, 3),
            CPUUsage = Random.Shared.NextDouble(),
            MemoryUsage = Random.Shared.NextDouble(),
            DiskUsage = Random.Shared.NextDouble(),
            NetworkUsage = Random.Shared.NextDouble(),
            GPU = Random.Shared.NextDouble(),
            GPUEngine = Lorem.Words(1, 3),
            PowerUsage = (ProcessPowerUsage)Enum.Parse(typeof(ProcessPowerUsage),
                Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length).ToString()),
            PowerUsageTrend = (ProcessPowerUsage)Enum.Parse(typeof(ProcessPowerUsage),
                Random.Shared.Next(0, Enum.GetNames(typeof(ProcessPowerUsage)).Length).ToString())
        }).ToList();
    }
}