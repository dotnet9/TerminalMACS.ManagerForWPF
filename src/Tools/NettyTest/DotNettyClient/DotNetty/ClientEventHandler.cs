using DotNetty.Transport.Channels;
using NettyModel.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNettyClient.DotNetty
{
    public class ClientEventHandler
    {
        /// <summary>
        /// 心跳间隔时间
        /// </summary>
        public const int PING_INTERVAL = 5;
        /// <summary>
        /// 消息发送重试次数
        /// </summary>
        private const int RETRY_SEND_TIME = 3;
        /// <summary>
        /// 消息发送时间间隔
        /// </summary>
        private const int DATA_SEND_INTERVAL = 5;
        /// <summary>
        /// 用于存放发送的ping包
        /// </summary>
        public static ConcurrentQueue<string> LstSendPings = new ConcurrentQueue<string>();
        /// <summary>
        /// 读取数据锁
        /// </summary>
        public static object LockOjb = new object();
        /// <summary>
        /// 用于存放需要发送的数据
        /// </summary>
        public static List<NettyBodyCounter> LstNeedSendDatas = new List<NettyBodyCounter>();
        /// <summary>
        /// 记录日志事件
        /// </summary>
        public static Action<string> RecordLogEvent;
        /// <summary>
        /// 从服务端收到数据
        /// </summary>
        public static Action<NettyBody> ReceiveEventFromClientEvent;

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="ctx"></param>
        public static void SendPingMsg(IChannelHandlerContext ctx)
        {
            // 发送的ping，超过一定范围，认为与服务端断开连接，需要重连
            if (LstSendPings.Count >= RETRY_SEND_TIME)
            {
                ctx.CloseAsync();
                RecordLogEvent?.Invoke($"{LstSendPings.Count} 次未收到心跳回应，重连服务器");
                LstSendPings.Clear();
                return;
            }
            string guid = System.Guid.NewGuid().ToString();
            LstSendPings.Enqueue(guid);
            ctx.WriteAndFlushAsync(NettyBody.ping(guid));
            RecordLogEvent?.Invoke($"发送心跳包，已发送{LstSendPings.Count} 次");
        }


        /// <summary>
        /// 发送数据到服务端
        /// </summary>
        /// <param name="nettyBody"></param>
        public static void SendData(NettyBody nettyBody)
        {
            try
            {
                lock (LockOjb)
                {
                    LstNeedSendDatas.Add(new NettyBodyCounter { NettyBody = nettyBody });
                }
            }
            catch (Exception ex)
            {
                RecordLogEvent?.Invoke($"发送数据异常：{ex.Message}");
            }
        }

        private static bool isRunning = false;
        /// <summary>
        /// 发送数据
        /// </summary>
        public static void RunSendData(IChannelHandlerContext ctx)
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
                        NettyBodyCounter sendEvent = null;
                        lock (LockOjb)
                        {
                            for (int i = LstNeedSendDatas.Count - 1; i >= 0; i--)
                            {
                                var tmpNettyBody = LstNeedSendDatas[i];
                                if (tmpNettyBody.TryCount >= RETRY_SEND_TIME)
                                {
                                    LstNeedSendDatas.Remove(tmpNettyBody);
                                    RecordLogEvent?.Invoke($"删除超时数据包(已发{tmpNettyBody.TryCount}次)：{JsonConvert.SerializeObject(tmpNettyBody.NettyBody)}");
                                }
                            }
                            sendEvent = LstNeedSendDatas.FirstOrDefault();
                        }
                        if (sendEvent != null)
                        {
                            ctx.WriteAndFlushAsync(sendEvent.NettyBody);
                            RecordLogEvent?.Invoke($"发送到服务端(已发{sendEvent.TryCount}次)：{JsonConvert.SerializeObject(sendEvent.NettyBody)}");
                            sendEvent.TryCount++;
                        }
                    }
                    catch (Exception ex2)
                    {
                        RecordLogEvent?.Invoke($"发送到服务端异常：{ex2.Message}");
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(DATA_SEND_INTERVAL));
                }
            });
        }
    }
}
