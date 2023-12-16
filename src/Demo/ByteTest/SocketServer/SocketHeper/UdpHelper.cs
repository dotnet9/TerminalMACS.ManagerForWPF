namespace SocketServer.SocketHeper;

internal class UdpHelper : ISocketBase
{
    private static readonly UdpClient Client = new();
    private static IPEndPoint? _udpIpEndPoint;
    private static bool _isRunSuccess;
    private BlockingCollection<INetObject> NeedSendCommands { get; } = new();

    public void Start(string ip, int port)
    {
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    var ipAddress = IPAddress.Parse(ip);
                    _udpIpEndPoint = new IPEndPoint(ipAddress, port);
                    Client.JoinMulticastGroup(ipAddress);
                    _isRunSuccess = true;

                    SendCommands();

                    Logger.Info($"Udp启动成功");
                    break;
                }
                catch (Exception ex)
                {
                    _isRunSuccess = false;
                    Logger.Warning($"运行Udp异常，1秒后将重新运行：{ex.Message}");
                    Thread.Sleep(1);
                }
            }
        });
    }

    public void Stop()
    {
        _isRunSuccess = false;
        try
        {
            Client.Close();
            Logger.Info($"停止Udp");
        }
        catch (Exception ex)
        {
            Logger.Warning($"停止Udp异常：{ex.Message}");
        }
    }

    public void SendCommand(INetObject command)
    {
        NeedSendCommands.Add(command);
    }

    public bool TryGetResponse(out INetObject? response)
    {
        response = null;
        return false;
    }

    private void SendCommands()
    {
        Task.Run(() =>
        {
            while (_isRunSuccess)
            {
                if (NeedSendCommands.TryTake(out var command))
                {
                    try
                    {
                        var buffer = command.Serialize(TcpHelper.SystemId);
                        Client.Send(buffer, buffer.Length, _udpIpEndPoint);
                    }
                    catch (Exception ex)
                    {
                        NeedSendCommands.Add(command);
                        Logger.Error($"发送命令{command.GetType().Name}失败，将排队重新发送: {ex.Message}");
                    }
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }
}