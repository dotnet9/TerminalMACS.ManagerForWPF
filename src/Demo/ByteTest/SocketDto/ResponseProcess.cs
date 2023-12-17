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
    /// 总数据大小
    /// </summary>
    [Key(1)]
    public int TotalSize { get; set; }

    /// <summary>
    /// 分页大小
    /// </summary>
    [Key(2)]
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    [Key(3)]
    public int PageCount { get; set; }

    /// <summary>
    /// 页索引
    /// </summary>
    [Key(4)]
    public int PageIndex { get; set; }

    /// <summary>
    /// 进程列表
    /// </summary>
    [Key(5)]
    public List<Process>? Processes { get; set; }
}