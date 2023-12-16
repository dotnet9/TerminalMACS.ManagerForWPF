namespace SocketNetObject.Models;

/// <summary>
/// 网络通信对象头部信息
/// </summary>
public class NetObjectHeadInfo
{
    /// <summary>
    /// 数据包大小
    /// </summary>
    public int BufferLen { get; set; }

    /// <summary>
    /// 系统标识
    /// </summary>
    public long SystemId { get; set; }

    /// <summary>
    /// 对象Id
    /// </summary>
    public byte ObjectId { get; set; }

    /// <summary>
    /// 对象版本号
    /// </summary>
    public byte ObjectVersion { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(BufferLen)}: {BufferLen}, {nameof(SystemId)}: {SystemId}，{nameof(ObjectId)}: {ObjectId}，{nameof(ObjectVersion)}: {ObjectVersion}";
    }
}