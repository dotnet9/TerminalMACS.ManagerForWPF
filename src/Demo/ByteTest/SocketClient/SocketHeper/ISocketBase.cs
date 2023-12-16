namespace SocketClient.SocketHelper;

/// <summary>
/// 套接字接口
/// </summary>
public interface ISocketBase
{
    /// <summary>
    /// 启动
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    void Start(string ip, int port);

    /// <summary>
    /// 停止
    /// </summary>
    void Stop();

    /// <summary>
    /// 发送命令
    /// </summary>
    /// <param name="command"></param>
    void SendCommand(INetObject command);

    /// <summary>
    /// 获取响应数据
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    bool TryGetResponse(out INetObject? response);
}