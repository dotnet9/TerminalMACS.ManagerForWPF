namespace SocketDto;

/// <summary>
/// 响应请求进程信息
/// </summary>
[NetObjectHead(4, 1)]
[MessagePackObject]
public class ResponseProcess : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    [Key(0)]
    public int TaskId { get; set; }

    /// <summary>
    /// 进程列表
    /// </summary>
    [Key(1)]
    public List<Process>? Processes { get; set; }
}