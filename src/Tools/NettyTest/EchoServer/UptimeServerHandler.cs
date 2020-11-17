using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoServer
{
    public class UptimeServerHandler : SimpleChannelInboundHandler<object>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            // discard
        }

        //捕获 异常，并输出到控制台后断开链接，提示：客户端意外断开链接，也会触发
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
