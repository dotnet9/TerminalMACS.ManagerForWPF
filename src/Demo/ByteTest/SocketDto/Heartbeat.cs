namespace SocketDto;

/// <summary>
/// TCP心跳包
/// </summary>
[NetObjectHead(255, 1)]
public class Heartbeat : INetObject
{
}