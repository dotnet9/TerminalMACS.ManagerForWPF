namespace SocketDto;

/// <summary>
/// 请求基本信息
/// </summary>
[NetObjectHead(1, 1)]
public class RequestBaseInfo : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    public int TaskId { get; set; }
}