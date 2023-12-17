namespace SocketClient.SocketHelper;

/// <summary>
/// 套接字接口
/// </summary>
public interface ISocketBase
{
    /// <summary>
    /// IP
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 是否已经开启
    /// </summary>
    public bool IsStarted { get; set; }

    /// <summary>
    /// 是否正在运行
    /// </summary>
    public bool IsRunning { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime ReceiveTime { get; set; }

    /// <summary>
    /// 启动
    /// </summary>
    void Start();

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