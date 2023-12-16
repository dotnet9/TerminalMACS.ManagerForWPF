using System.ComponentModel;

namespace SocketCore.SysProcess.Models;

/// <summary>
/// 进程类型
/// </summary>
public enum ProcessType
{
    [Description("应用")] Application,
    [Description("后台进程")] BackgroundProcess
}