using System.ComponentModel;

namespace SocketCore.SysProcess.Models;

/// <summary>
/// 进程运行状态
/// </summary>
public enum ProcessStatus
{
    [Description("正常运行")] Running,
    [Description("效率模式")] EfficiencyMode,
    [Description("挂起")] Pending
}