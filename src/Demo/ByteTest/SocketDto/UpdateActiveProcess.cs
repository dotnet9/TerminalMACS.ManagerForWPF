namespace SocketDto;

/// <summary>
/// 更新进程变化信息
/// </summary>
[NetObjectHead(6, 1)]
[MessagePackObject]
public class UpdateActiveProcess : INetObject
{
    /// <summary>
    /// 进程列表
    /// </summary>
    [Key(0)]
    public List<ActiveProcess>? Processes { get; set; }
}