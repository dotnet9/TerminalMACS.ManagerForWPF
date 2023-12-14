namespace SocketDto;

/// <summary>
/// 响应请求进程信息
/// </summary>
public class ResponseProcess
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// 进程总数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 页索引
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 页数
    /// </summary>

    public int PageSize { get; set; }

    /// <summary>
    /// 进程列表
    /// </summary>
    public List<Process>? Processes { get; set; }
}