namespace SocketDto;

/// <summary>
/// 更新进程信息
/// </summary>
[NetObjectHead(5, 1)]
[MessagePackObject]
public class UpdateProcess : INetObject
{
    /// <summary>
    /// 进程列表
    /// </summary>
    [Key(0)]
    public List<Process>? Processes { get; set; }
}