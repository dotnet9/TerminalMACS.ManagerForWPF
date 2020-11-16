using DotNetty.Transport.Channels;
using EventBus;
using MessagePack;
using NettyModel.Event;
using Newtonsoft.Json;
using System;

namespace EchoServer
{
    /// <summary>
    /// 因为服务器只需要响应传入的消息，所以只需要实现ChannelHandlerAdapter就可以了
    /// </summary>
    public class NettyServerHandler : SimpleChannelInboundHandler<Object>
    {
        private IChannelHandlerContext _Socket;
        /// <summary>
        /// Socket
        /// </summary>
        public IChannelHandlerContext Socket
        {
            get { return _Socket; }
            set
            {
                try
                {
                    _Socket = value;
                }
                catch (Exception ex)
                {
                }
            }
        }
        public void SendData(TestEvent testEvent)
        {
            try
            {
                Socket.WriteAndFlushAsync(testEvent);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            TestEvent testEvent = MessagePackSerializer.Deserialize<TestEvent>(MessagePackSerializer.Serialize(msg));
            Console.WriteLine("服务端接收到消息:" + JsonConvert.SerializeObject(testEvent));
            SimpleEventBus.GetDefaultEventBus().Post(testEvent.ToString(), TimeSpan.Zero);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            base.ChannelReadComplete(context);
            context.Flush();
            Console.WriteLine("ChannelReadComplete:" + context);
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);
            Console.WriteLine("Server ChannelRegistered:" + context);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Socket = context;
            Console.WriteLine("Server channelActive:" + context);
            SimpleEventBus.GetDefaultEventBus().Post("建立连接：" + context, TimeSpan.Zero);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            Console.WriteLine("Server ChannelInactive:" + context);
            SimpleEventBus.GetDefaultEventBus().Post("连接断开：" + context, TimeSpan.Zero);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);
            Console.WriteLine("Server ChannelUnregistered:" + context);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
