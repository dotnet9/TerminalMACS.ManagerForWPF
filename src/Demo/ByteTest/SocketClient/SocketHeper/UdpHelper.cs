namespace SocketClient.SocketHeper;

internal class UdpHelper : ISocketBase
{
    private static readonly UdpClient Client = new();
    private static bool _isRunSuccess;

    private static readonly BlockingCollection<byte[]>
        ReceivedBuffers = new(new ConcurrentQueue<byte[]>());

    private static readonly BlockingCollection<UpdateActiveProcess> ReceivedResponse = new();

    public void Start(string ip, int port)
    {
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                    Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                    // 任意IP+广播端口，0是任意端口
                    Client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

                    // 加入组播
                    Client.JoinMulticastGroup(IPAddress.Parse(ip));
                    _isRunSuccess = true;
                    AnalyzeData();

                    var remoteEp = new IPEndPoint(IPAddress.Any, 0);
                    while (true)
                    {
                        if (Client.Client == null || Client.Available < 0 || !_isRunSuccess)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(10));
                            continue;
                        }

                        if (!_isRunSuccess)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(10));
                            continue;
                        }

                        var data = Client.Receive(ref remoteEp);
                        ReceivedBuffers.Add(data);
                        Thread.Sleep(TimeSpan.FromMilliseconds(1));
                    }
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
    }

    public bool TryGetResponse(out INetObject? response)
    {
        bool result = ReceivedResponse.TryTake(out var updateActiveProcess);
        response = updateActiveProcess;
        return result;
    }

    private static void AnalyzeData()
    {
        Task.Run(() =>
        {
            while (_isRunSuccess)
            {
                if (ReceivedBuffers.TryTake(out var buffer))
                {
                    var sw = Stopwatch.StartNew();
                    var readIndex = 0;
                    try
                    {
                        if (!SerializeHelper.ReadHead(buffer, ref readIndex, out var netObjectInfo)
                            || buffer.Length != netObjectInfo!.BufferLen)
                        {
                            continue;
                        }

                        ReceivedResponse.Add(SerializeHelper.Deserialize<UpdateActiveProcess>(buffer));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"解析实时数据异常，将放弃处理该包：{ex.Message}");
                    }
                    finally
                    {
                        sw.Stop();
                        Logger.Info($"解析UDP包用时：{sw.ElapsedMilliseconds}ms");
                    }
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
        });
    }
}