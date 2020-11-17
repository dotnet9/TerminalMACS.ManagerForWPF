using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using EventBus;
using MessagePack;
using NettyModel.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EchoServer
{
    /// <summary>
    /// 因为服务器只需要响应传入的消息，所以只需要实现ChannelHandlerAdapter就可以了
    /// </summary>
    public class NettyServerHandler : SimpleChannelInboundHandler<Object>
    {
        private IChannelHandlerContext channelHandlerContext;
        private bool isConnect = false;


        /// <summary>
        /// 发送数据到客户端
        /// </summary>
        /// <param name="testEvent"></param>
        public void SendData(TestEvent testEvent)
        {
            try
            {
                if (channelHandlerContext == null)
                {
                    return;
                }
                channelHandlerContext.WriteAndFlushAsync(testEvent);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 收到客户端回应
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="msg"></param>
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            try
            {
                TestEvent testEvent = MessagePackSerializer.Deserialize<TestEvent>(MessagePackSerializer.Serialize(msg));

                // 服务端收到心跳，直接回应
                if (testEvent.code == EventCode.Ping)
                {
                    ctx.WriteAndFlushAsync(msg);
                    Console.WriteLine("收到心跳并原文回应");
                    return;
                }
                Console.WriteLine("服务端接收到消息:" + JsonConvert.SerializeObject(testEvent));

                // 回应收到消息成功
                testEvent.code = EventCode.OK;
                ctx.WriteAndFlushAsync(testEvent);
                SimpleEventBus.GetDefaultEventBus().Post(testEvent, TimeSpan.Zero);
            }
            catch (Exception ex)
            {

            }
        }

        static volatile IChannelGroup groups;
        //客户端连接进来时
        public override void HandlerAdded(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context}上线.");
            //SimpleEventBus.GetDefaultEventBus().Post("客户端连接进来：" + context, TimeSpan.Zero);
            base.HandlerAdded(context);

            IChannelGroup g = groups;
            if (g == null)
            {
                lock (this)
                {
                    if (groups == null)
                    {
                        g = groups = new DefaultChannelGroup(context.Executor);
                    }
                }
            }

            g.Add(context.Channel);
            //groups.WriteAndFlushAsync($"欢迎{context.Channel.RemoteAddress}加入.");
        }

        //客户端下线断线时
        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context}下线.");
            base.HandlerRemoved(context);

            groups.Remove(context.Channel);
            //groups.WriteAndFlushAsync($"恭送{context.Channel.RemoteAddress}离开.");
            //SimpleEventBus.GetDefaultEventBus().Post("下线：" + context, TimeSpan.Zero);
        }

        //服务器监听到客户端活动时
        public override void ChannelActive(IChannelHandlerContext context)
        {
            channelHandlerContext = context;
            Console.WriteLine($"客户端{context.Channel.RemoteAddress}在线.");
            //SimpleEventBus.GetDefaultEventBus().Post("在线：" + context, TimeSpan.Zero);
            base.ChannelActive(context);
        }

        //服务器监听到客户端不活动时
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context.Channel.RemoteAddress}离线了.");
            //SimpleEventBus.GetDefaultEventBus().Post("离线：" + context, TimeSpan.Zero);
            base.ChannelInactive(context);
        }
        private int lossConnectCount = 0;

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            Console.WriteLine("已经15秒未收到客户端的消息了！");
            if (evt is IdleStateEvent eventState)
            {
                if (eventState.State == IdleState.ReaderIdle)
                {
                    lossConnectCount++;
                    if (lossConnectCount > 2)
                    {
                        Console.WriteLine("关闭这个不活跃通道！");
                        context.CloseAsync();
                    }
                }
            }
            else
            {
                base.UserEventTriggered(context, evt);
            }
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
