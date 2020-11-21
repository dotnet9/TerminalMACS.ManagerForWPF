using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using MessagePack;
using NettyModel.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotNettyClient.DotNetty
{
    public class NettyClientChannelHandler : SimpleChannelInboundHandler<Object>
    {
        private string serverIP;
        private int serverPort;
        public NettyClientChannelHandler(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
        }

        public override bool IsSharable => true;//标注一个channel handler可以被多个channel安全地共享。

        /// <summary>
        /// 收到服务端消息
        /// </summary>
        /// <param name="ctx">通道处理上下文</param>
        /// <param name="msg">接收内容</param>
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            try
            {
                NettyBody testEvent = MessagePackSerializer.Deserialize<NettyBody>(MessagePackSerializer.Serialize(msg));

                // 收到发送给服务端的心跳包，服务端回应
                if (testEvent.code == (int)NettyCodeEnum.Ping)
                {
                    ClientEventHandler.LstSendPings.Clear();
                    ClientEventHandler.RecordLogEvent?.Invoke("收到Android端心跳回应");
                    return;
                }
                // 发送数据给服务端，服务端处理成功回应
                if (testEvent.code == (int)NettyCodeEnum.OK)
                {
                    lock (ClientEventHandler.LockOjb)
                    {
                        ClientEventHandler.LstNeedSendDatas.RemoveAll(cu => cu.NettyBody.reqId == testEvent.reqId);
                    }
                    return;
                }
                // 收到服务端发送过来的聊天内容
                if (testEvent.code == (int)NettyCodeEnum.Chat)
                {
                    ClientEventHandler.ReceiveEventFromClientEvent?.Invoke(testEvent);
                }
                var eventMsg = JsonConvert.SerializeObject(testEvent);
                ClientEventHandler.RecordLogEvent?.Invoke($"收到Android端消息：{eventMsg}");
            }
            catch (Exception ex)
            {
                ClientEventHandler.RecordLogEvent?.Invoke($"读取数据异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 消息接收完毕
        /// </summary>
        /// <param name="context">通道处理上下文</param>
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            base.ChannelReadComplete(context);
            context.Flush();
            ClientEventHandler.RecordLogEvent?.Invoke("ChannelReadComplete");
        }

        /// <summary>
        /// 注册通道
        /// </summary>
        /// <param name="context">通道处理上下文</param>
        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);
            ClientEventHandler.RecordLogEvent?.Invoke($"注册通道：{context.Channel.RemoteAddress}");
        }

        /// <summary>
        /// 通道激活
        /// </summary>
        /// <param name="context">通道处理上下文</param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            ClientEventHandler.RecordLogEvent?.Invoke($"通道激活：{context.Channel.RemoteAddress}");
            ClientEventHandler.IsConnect = true;
            ClientEventHandler.RunSendData(context);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            ClientEventHandler.RecordLogEvent?.Invoke($"断开连接：{context.Channel.RemoteAddress}"); 
            ClientEventHandler.IsConnect = false;
        }

        /// <summary>
        /// 注销通道
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);
            ClientEventHandler.RecordLogEvent?.Invoke($"注销通道：{context.Channel.RemoteAddress}");
            ClientEventHandler.IsConnect = false;
            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (!ClientEventHandler.IsConnect)
            {
                NettyClient nettyClient = new NettyClient(serverIP, serverPort);
                nettyClient.ConnectServer().Wait();
            }
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            ClientEventHandler.RecordLogEvent?.Invoke($"Exception: {exception.Message}");
            ClientEventHandler.IsConnect = false;
            context.CloseAsync();
        }

        /// <summary>
        /// 读写超时通知
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="evt"></param>
        public override void UserEventTriggered(IChannelHandlerContext ctx, object evt)
        {
            base.UserEventTriggered(ctx, evt);
            ClientEventHandler.SendPingMsg(ctx);
        }
    }
}
