namespace SocketDto;

/// <summary>
/// TCP心跳包
/// </summary>
[NetObjectHead(255, 1)]
[MessagePackObject]
public class Heartbeat : INetObject
{
}