using SocketNetObject.Models;
using System.Net.Sockets;

namespace SocketNetObject;

public static class SocketHelper
{
    public static bool ReadPacket(this Socket socket, out byte[] buffer, out NetObjectHeadInfo netObject)
    {
        // 1、接收数据包
        var lenBuffer = ReceiveBuffer(socket, 4);
        var bufferLen = BitConverter.ToInt32(lenBuffer, 0);

        var exceptLenBuffer = ReceiveBuffer(socket, bufferLen - 4);

        buffer = new byte[bufferLen];

        Array.Copy(lenBuffer, buffer, 4);
        Buffer.BlockCopy(exceptLenBuffer, 0, buffer, 4, bufferLen - 4);

        // 2、解析数据包
        var readIndex = 0;
        return SerializeHelper.ReadHead(buffer, ref readIndex, out netObject);
    }

    private static byte[] ReceiveBuffer(Socket client, int count)
    {
        var buffer = new byte[count];
        var bytesReadAllCount = 0;
        while (bytesReadAllCount != count)
        {
            bytesReadAllCount +=
                client.Receive(buffer, bytesReadAllCount, count - bytesReadAllCount, SocketFlags.None);
        }

        return buffer;
    }
}