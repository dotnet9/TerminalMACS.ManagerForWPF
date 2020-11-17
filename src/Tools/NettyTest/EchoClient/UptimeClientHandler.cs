using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace EchoClient
{
    public class UptimeClientHandler : SimpleChannelInboundHandler<object>
    {
        long startTime = -1;

        //ChannelActive:活跃状态，可接收和发送数据
        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            if (startTime < 0)
            {
                startTime = GetTimeStamp();
            }
            Console.WriteLine("Connected to: " + ctx.Channel.RemoteAddress);
        }

        protected override void ChannelRead0(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                Console.WriteLine("Received from server: " + byteBuffer.ToString(Encoding.UTF8));
            }
            context.WriteAsync(message);
        }

        public override void UserEventTriggered(IChannelHandlerContext ctx, object evt)
        {
            if (!(evt is IdleStateEvent))
            {
                return;
            }

            IdleStateEvent e = evt as IdleStateEvent;
            if (e.State == IdleState.ReaderIdle)
            {
                // The connection was OK but there was no traffic for last period.
                Console.WriteLine("Disconnecting due to no inbound traffic");
                ctx.CloseAsync();
            }
        }

        //channelInactive： 处于非活跃状态，没有连接到远程主机。
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("Disconnected from: " + context.Channel.RemoteAddress);
        }

        //channelUnregistered： 已创建但未注册到一个 EventLoop。
        public override void ChannelUnregistered(IChannelHandlerContext context)
        {

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

    }
}
