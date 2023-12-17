namespace SocketDto;

/// <summary>
/// 请求基本信息
/// </summary>
[NetObjectHead(1, 1)]
[MessagePackObject]
public class RequestBaseInfo : INetObject
{
    /// <summary>
    /// 任务Id
    /// </summary>
    [Key(0)]
    public int TaskId { get; set; }
}