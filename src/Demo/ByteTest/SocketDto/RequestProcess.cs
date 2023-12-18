namespace SocketDto;

/// <summary>
/// 请求进程信息
/// </summary>
[NetObjectHead(3, 1)]
public class RequestProcess : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public int TaskId { get; set; }
}