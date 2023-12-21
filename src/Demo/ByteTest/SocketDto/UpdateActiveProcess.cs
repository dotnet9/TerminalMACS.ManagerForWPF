namespace SocketDto;

/// <summary>
/// 更新进程变化信息，序列化和反序列不能加压缩，部分双精度因为有效位数太长，可能导致UDP包过大而发送失败，所以UDP包不要加压缩
/// </summary>
[NetObjectHead(6, 1)]
public class UpdateActiveProcess : INetObject
{
    /// <summary>
    /// 进程列表
    /// </summary>
    public List<ActiveProcess>? Processes { get; set; }
}