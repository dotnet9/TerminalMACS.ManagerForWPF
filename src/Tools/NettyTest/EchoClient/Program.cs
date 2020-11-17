using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyModel.Coder;
using NettyModel.Event;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunClientAsync().Wait();
        }
        static EchoClientHandler echoClientHandler = null;
        // 重连时休眠时间
        private const int RECONNECT_DELAY = 5;
        // 未收到服务端回应超时时间
        private const int READ_TIMEOUT = 10;
        static UptimeClientHandler uptimeClientHandler = new UptimeClientHandler();

        static async Task RunClientAsync()
        {
            var group = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        // Tcp粘包处理，添加一个LengthFieldBasedFrameDecoder解码器，它会在解码时按照消息头的长度来进行解码。
                        pipeline.AddLast("frameDecoder", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                        // MessagePack解码器，消息进来后先由frameDecoder处理，再给msgPackDecoder处理
                        pipeline.AddLast("msgPackDecoder", new MessagePackDecoder());
                        // Tcp粘包处理，添加一个
                        // LengthFieldPrepender编码器，它会在ByteBuf之前增加4个字节的字段，用于记录消息长度。
                        pipeline.AddLast("frameEncoder", new LengthFieldPrepender(4));
                        // MessagePack编码器，消息发出之前先由frameEncoder处理，再给msgPackEncoder处理
                        pipeline.AddLast("msgPackEncoder", new MessagePackEncoder());
                        // 消息处理handler
                        echoClientHandler = new EchoClientHandler();
                        pipeline.AddLast("handler", echoClientHandler);
                        pipeline.AddLast(new IdleStateHandler(READ_TIMEOUT, 0, 0), uptimeClientHandler);
                    }));
                IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10086));

                ThreadPool.QueueUserWorkItem(sen =>
                {
                    while (true)
                    {
                        if (echoClientHandler != null && echoClientHandler.Socket != null && echoClientHandler.Socket.Channel.Active)
                        {
                            echoClientHandler.SendData(new TestEvent()
                            {
                                code = EventCode.OK,
                                time = GetCurrentTimeStamp(),
                                msg = "客户端请求",
                                fromId = "",
                                reqId = $"",
                                data = $"客户端时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
                            });
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                });

                Console.ReadLine();
                await clientChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await group.ShutdownGracefullyAsync();
            }
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimeStamp()
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1);    // 当地时区
            return (long)(DateTime.UtcNow - startTime).TotalMilliseconds;   // 相差毫秒数
        }
    }
}
