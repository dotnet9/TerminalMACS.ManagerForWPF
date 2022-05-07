using Masuit.Tools;
using Masuit.Tools.DateTimeExt;
using Masuit.Tools.Hardware;
using Masuit.Tools.Systems;

public class Program
{
    static void Main(string[] args)
    {
        PerfCounter.Init();
        Task.Run(() =>
            {
                while (true)
                {
                    var count = PerfCounter.List.LastOrDefault();
                    if (count != null)
                    {
                        Console.WriteLine(count);
                    }


                    Thread.Sleep(5000);
                }
            }
        );

        Console.ReadKey();
    }
}

public static class PerfCounter
{
    public static ConcurrentLimitedQueue<PerformanceCounter> List { get; set; } = new(50000);
    public static readonly DateTime StartTime = DateTime.Now;

    public static void Init()
    {
        Task.Run(() =>
        {
            int errorCount = 0;
            while (true)
            {
                try
                {
                    List.Enqueue(GetCurrentPerformanceCounter());
                }
                catch (Exception e)
                {
                    if (errorCount > 20)
                    {
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    errorCount++;
                }

                Thread.Sleep(5000);
            }
        });
    }

    public static PerformanceCounter GetCurrentPerformanceCounter()
    {
        var time = DateTime.Now.GetTotalMilliseconds();
        var load = SystemInfo.CpuLoad;
        var mem =
            (1 - SystemInfo.MemoryAvailable.ConvertTo<float>() / SystemInfo.PhysicalMemory.ConvertTo<float>()) *
            100;

        var read = SystemInfo.GetDiskData(DiskData.Read) / 1024f;
        var write = SystemInfo.GetDiskData(DiskData.Write) / 1024;

        var up = SystemInfo.GetNetData(NetData.Received) / 1024;
        var down = SystemInfo.GetNetData(NetData.Sent) / 1024;
        return new PerformanceCounter()
        {
            Time = time,
            CpuLoad = load,
            MemoryUsage = mem,
            DiskRead = read,
            DiskWrite = write,
            Download = down,
            Upload = up
        };
    }
}


/// <summary>
/// 性能计数器
/// </summary>
public class PerformanceCounter
{
    /// <summary>
    /// 当前时间戳
    /// </summary>
    public long Time { get; set; }

    /// <summary>
    /// CPU当前负载
    /// </summary>
    public float CpuLoad { get; set; }

    /// <summary>
    /// 内存使用率
    /// </summary>
    public float MemoryUsage { get; set; }

    /// <summary>
    /// 磁盘读
    /// </summary>
    public float DiskRead { get; set; }

    /// <summary>
    /// 磁盘写
    /// </summary>
    public float DiskWrite { get; set; }

    /// <summary>
    /// 网络上行
    /// </summary>
    public float Upload { get; set; }

    /// <summary>
    /// 网络下行
    /// </summary>
    public float Download { get; set; }

    public override string ToString()
    {
        return
            $"当前时间戳: {Time}, CPU当前负载: {CpuLoad * 10}, 内存使用率: {MemoryUsage}, 磁盘读: {DiskRead}, 磁盘写: {DiskWrite}, 网络上行: {Upload}, 网络下行: {Download}";
    }
}