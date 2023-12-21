using System.ComponentModel;

namespace SocketCore.SysProcess.Models;

public enum ProcessRunningStatus
{
    [Description("未运行")] None,
    [Description("已运行")] Running,
}