using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using MessagePack;
using NettyModel.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DotNettyClient.DotNetty
{
    public class EchoClientHandler : SimpleChannelInboundHandler<Object>
    {
        public event Action DisconnectServer;
        public event Action<TestEvent> ReceiveEventFromClientEvent;
        private IChannelHandlerContext channelHandlerContext;
        private List<TestEventCount> lstNeedSendDatas = new List<TestEventCount>();   // 用于存放需要发送的数据
        private static object lockOjb = new object();                       // 读取数据锁
        private List<TestEvent> lstSendPings = new List<TestEvent>();       // 用于存放发送的ping包
        private const int MAX_NO_RESPONSE_PING_COUNT = 7;                   // 未收到ping回应的数据包最大个数
        private bool isConnect = false;

        /// <summary>
        /// 发送数据到服务端
        /// </summary>
        /// <param name="testEvent"></param>
        public void SendData(TestEvent testEvent)
        {
            try
            {
                lock (lockOjb)
                {
                    lstNeedSendDatas.Add(new TestEventCount { TestEvent = testEvent });
                }
                if (channelHandlerContext == null)
                {
                    return;
                }
                SendData();
            }
            catch (Exception ex)
            {

            }
        }

        private bool isRunning = false;
        /// <summary>
        /// 发送数据
        /// </summary>
        private void SendData()
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            ThreadPool.QueueUserWorkItem(sen =>
            {
                while (true)
                {
                    try
                    {
                        TestEventCount sendEvent = null;
                        lock (lockOjb)
                        {
                            sendEvent = lstNeedSendDatas.FirstOrDefault(cu => cu.TryCount < TestEventCount.MAX_TRY_COUNT);
                        }
                        if (sendEvent != null)
                        {
                            channelHandlerContext.WriteAndFlushAsync(sendEvent.TestEvent);
                            sendEvent.TryCount++;
                        }
                    }
                    catch (Exception ex2)
                    {

                    }
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            });
        }

        public override bool IsSharable => true;//标注一个channel handler可以被多个channel安全地共享。

        /// <summary>
        /// 收到服务端回应消息
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="msg"></param>
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            try
            {
                TestEvent testEvent = MessagePackSerializer.Deserialize<TestEvent>(MessagePackSerializer.Serialize(msg));
                if (testEvent.code == EventCode.Ping)
                {
                    lstSendPings.Clear();
                    Console.WriteLine("收到Android端心跳回应");
                    return;
                }
                else if (testEvent.code == EventCode.OK)
                {
                    lock (lockOjb)
                    {
                        lstNeedSendDatas.RemoveAll(cu => cu.TestEvent.reqId == testEvent.reqId);
                    }
                }
                var eventMsg = JsonConvert.SerializeObject(testEvent);
                Console.WriteLine($"收到Android端消息：{eventMsg}");
                ReceiveEventFromClientEvent?.Invoke(testEvent);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 读写超时通知
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="evt"></param>
        public override void UserEventTriggered(IChannelHandlerContext ctx, object evt)
        {
            if (!(evt is IdleStateEvent))
            {
                return;
            }

            // 发送的ping，超过一定范围，认为与服务端断开连接，需要重连
            if (lstSendPings.Count >= MAX_NO_RESPONSE_PING_COUNT)
            {
                ctx.CloseAsync();
                return;
            }

            // 向服务端发送心跳
            var testEvent = new TestEvent()
            {
                code = EventCode.Ping,
                reqId = Guid.NewGuid().ToString()
            };
            lstSendPings.Add(testEvent);
            ctx.WriteAndFlushAsync(testEvent);
        }


        public override void HandlerAdded(IChannelHandlerContext context)
        {
            Console.WriteLine($"服务端{context}上线.");
            base.HandlerAdded(context);
        }

        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            Console.WriteLine($"服务端{context}下线.");
            base.HandlerRemoved(context);
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
            Console.WriteLine("Client ChannelRegistered:" + context);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            channelHandlerContext = context;
            Console.WriteLine("Client channelActive:" + context);
            Console.WriteLine("我是客户端.");
            Console.WriteLine($"连接至服务端{context}.");
            isConnect = true;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            Console.WriteLine("Client ChannelInactive:" + context);
            isConnect = false;
            //SimpleEventBus.GetDefaultEventBus().Post("连接断开：" + context, TimeSpan.Zero);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);

            DisconnectServer?.Invoke();

            Console.WriteLine("Client ChannelUnregistered:" + context);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
