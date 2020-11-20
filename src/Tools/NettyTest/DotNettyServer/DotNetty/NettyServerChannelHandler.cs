using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using MessagePack;
using NettyModel.Event;
using Newtonsoft.Json;
using System;

namespace DotNettyServer.DotNetty
{
    /// <summary>
    /// 因为服务器只需要响应传入的消息，所以只需要实现ChannelHandlerAdapter就可以了
    /// </summary>
    public class NettyServerChannelHandler : SimpleChannelInboundHandler<Object>
    {
        private IChannelHandlerContext channelHandlerContext;
        public event Action<NettyBody> ReceiveEventFromClientEvent;
        public event Action<string> RecordLogEvent;


        /// <summary>
        /// 发送数据到客户端
        /// </summary>
        /// <param name="testEvent"></param>
        public void SendData(NettyBody testEvent)
        {
            try
            {
                if (channelHandlerContext == null)
                {
                    RecordLogEvent?.Invoke($"未连接客户端，无法发送数据");
                    return;
                }
                channelHandlerContext.WriteAndFlushAsync(testEvent);
            }
            catch (Exception ex)
            {
                RecordLogEvent?.Invoke($"发送数据异常：{ex.Message}");
            }
        }
        public override bool IsSharable => true;//标注一个channel handler可以被多个channel安全地共享。

        /// <summary>
        /// 收到客户端回应
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="msg"></param>
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            try
            {
                NettyBody testEvent = MessagePackSerializer.Deserialize<NettyBody>(MessagePackSerializer.Serialize(msg));

                // 服务端收到心跳，直接回应
                if (testEvent.code == (int)NettyCodeEnum.Ping)
                {
                    ctx.WriteAndFlushAsync(msg);
                    RecordLogEvent?.Invoke($"收到心跳并原文回应");
                    return;
                }
                if (testEvent.code == (int)NettyCodeEnum.Chat)
                {
                    // 回应收到消息成功
                    testEvent.code = (int)NettyCodeEnum.OK;
                    ctx.WriteAndFlushAsync(testEvent);
                    ReceiveEventFromClientEvent?.Invoke(testEvent);
                    return;
                }
                RecordLogEvent?.Invoke($"服务端接收到消息:" + JsonConvert.SerializeObject(testEvent));

            }
            catch (Exception ex)
            {
                RecordLogEvent?.Invoke($"收到客户端消息，处理失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 消息读取完成
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            base.ChannelReadComplete(context);
            context.Flush();
            RecordLogEvent?.Invoke($"ChannelReadComplete:" + context);
        }

        /// <summary>
        /// 注册通道
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);
            RecordLogEvent?.Invoke($"注册通道：{context.Channel.RemoteAddress}");
        }

        /// <summary>
        /// 通道激活
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            channelHandlerContext = context;
            RecordLogEvent?.Invoke($"通道激活：{context.Channel.RemoteAddress}");
            base.ChannelActive(context);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            RecordLogEvent?.Invoke($"断开连接：{context.Channel.RemoteAddress}");
            base.ChannelInactive(context);
        }

        /// <summary>
        /// 注销通道
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);
            RecordLogEvent?.Invoke($"注销通道：{context.Channel.RemoteAddress}");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            RecordLogEvent?.Invoke($"异常：{exception.Message}");
            context.CloseAsync();
        }
    }
}
