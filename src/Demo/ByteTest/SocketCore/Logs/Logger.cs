using System.Collections.Concurrent;
using SocketCore.Logs.Models;

namespace SocketCore.Logs;

public static class Logger
{
    internal static readonly BlockingCollection<LogInfo> Logs = new();

    public static void Debug(string content)
    {
        Logs.Add(new LogInfo(LogType.Debug, content, DateTime.Now));
    }

    public static void Info(string content)
    {
        Logs.Add(new LogInfo(LogType.Info, content, DateTime.Now));
    }

    public static void Warning(string content)
    {
        Logs.Add(new LogInfo(LogType.Warning, content, DateTime.Now));
    }

    public static void Error(string content)
    {
        Logs.Add(new LogInfo(LogType.Error, content, DateTime.Now));
    }
}