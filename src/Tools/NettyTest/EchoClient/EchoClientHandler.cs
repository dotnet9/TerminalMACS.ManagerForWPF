using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoClient
{
    public class EchoClientHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        /// <summary>
        /// Read0是DotNetty特有的对于Read方法的封装
        /// 封装实现了：
        /// 1. 返回的message的泛型实现
        /// 2. 丢弃非该指定泛型的信息
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="msg"></param>
        protected override void ChannelRead0(IChannelHandlerContext ctx, IByteBuffer msg)
        {
            if (msg != null)
            {
                Console.WriteLine("收到服务端数据:" + msg.ToString(Encoding.UTF8));
            }
            ctx.WriteAsync(Unpooled.CopiedBuffer(msg));
        }
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("发送当前时间");
            context.WriteAndFlushAsync(Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))));
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine(exception);
            context.CloseAsync();
        }
    }
}
