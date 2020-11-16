using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using MessagePack;
using NettyModel;
using NettyModel.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EchoServer
{
    /// <summary>
    /// 因为服务器只需要响应传入的消息，所以只需要实现ChannelHandlerAdapter就可以了
    /// </summary>
    public class NettyClientHandler : ChannelHandlerAdapter
    {
        int i = 0;
        /// <summary>
        /// 每个传入消息都会调用
        /// 处理传入的消息需要复写这个方法
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="msg"></param>
        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            object[] values = (object[])msg;
            int index = Convert.ToInt32(values[0]);//获取消息类别，第一个字段为EventType，这是约定好的
            TestEvent testEvent = MessagePackSerializer.Deserialize<TestEvent>(MessagePackSerializer.Serialize(msg));
            Console.WriteLine("Client接收到消息:" + JsonConvert.SerializeObject(testEvent));
            //SimpleEventBus.GetDefaultEventBus().Post(testEvent.ToString(), TimeSpan.Zero);

            //IByteBuffer message = msg as IByteBuffer;
            //Console.WriteLine("收到客户端信息：" + message.ToString(Encoding.UTF8));
            //ctx.WriteAsync(Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes($"收到客户端消息，回应：{i++}")));
        }
        /// <summary>
        /// 批量读取中的最后一条消息已经读取完成
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }
        /// <summary>
        /// 发生异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine(exception);
            context.CloseAsync();
        }
    }
}
