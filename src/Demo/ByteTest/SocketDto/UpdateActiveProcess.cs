namespace SocketDto;

/// <summary>
/// 更新进程变化信息
/// </summary>
public class UpdateActiveProcess
{
    /// <summary>
    /// 进程列表
    /// </summary>
    public List<ActiveProcess>? Processes { get; set; }
}