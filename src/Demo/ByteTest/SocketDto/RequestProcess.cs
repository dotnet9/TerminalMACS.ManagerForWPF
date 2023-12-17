namespace SocketDto;

/// <summary>
/// 请求进程信息
/// </summary>
[NetObjectHead(3, 1)]
[MessagePackObject]
public class RequestProcess : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    [Key(0)]
    public int TaskId { get; set; }
}