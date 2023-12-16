namespace SocketDto;

/// <summary>
/// 响应请求进程信息
/// </summary>
[NetObjectHead(4, 1)]
public class ResponseProcess : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// 进程列表
    /// </summary>
    public List<Process>? Processes { get; set; }
}