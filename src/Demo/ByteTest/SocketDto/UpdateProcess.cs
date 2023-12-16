namespace SocketDto;

/// <summary>
/// 更新进程信息
/// </summary>
[NetObjectHead(5, 1)]
public class UpdateProcess : INetObject
{
    /// <summary>
    /// 进程列表
    /// </summary>
    public List<Process>? Processes { get; set; }
}