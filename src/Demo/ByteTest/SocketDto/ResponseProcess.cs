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
    /// 总数据大小
    /// </summary>
    public int TotalSize { get; set; }

    /// <summary>
    /// 分页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// 页索引
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 进程列表
    /// </summary>
    public List<Process>? Processes { get; set; }
}