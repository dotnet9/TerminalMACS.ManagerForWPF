using System.Collections.Concurrent;

namespace SocketCore.Logs;

public static class Logger
{
    internal static readonly ConcurrentQueue<LogInfo> Logs = new ConcurrentQueue<LogInfo>();

    public static void Debug(string content)
    {
        Logs.Enqueue(new LogInfo(LogType.Debug, content, DateTime.Now));
    }

    public static void Info(string content)
    {
        Logs.Enqueue(new LogInfo(LogType.Info, content, DateTime.Now));
    }

    public static void Warning(string content)
    {
        Logs.Enqueue(new LogInfo(LogType.Warning, content, DateTime.Now));
    }

    public static void Error(string content)
    {
        Logs.Enqueue(new LogInfo(LogType.Error, content, DateTime.Now));
    }
}