using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyModel;
using NettyModel.Coder;
using NettyModel.Event;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServerAsync().Wait();
        }
        static NettyServerHandler nettyServerHandler = null;

        static async Task RunServerAsync()
        {
            IEventLoopGroup eventLoop;
            eventLoop = new MultithreadEventLoopGroup();
            try
            {
                // 服务器引导程序
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(eventLoop);
                bootstrap.Channel<TcpServerSocketChannel>();
                bootstrap.ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
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
                    nettyServerHandler = new NettyServerHandler();
                    // 消息处理handler
                    pipeline.AddLast("handler", nettyServerHandler);
                }));
                IChannel boundChannel = await bootstrap.BindAsync(10086);
                ThreadPool.QueueUserWorkItem(sen =>
                {
                    while (true)
                    {
                        if (nettyServerHandler != null && nettyServerHandler.Socket.Channel.Active)
                        {
                            nettyServerHandler.SendData(new TestEvent()
                            {
                                code = EventCode.OK,
                                time = UtilHelper.GetCurrentTimeStamp(),
                                msg = "服务器推送",
                                fromId = "",
                                reqId = $"",
                                data = $"服务器时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
                            });
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                });
                Console.ReadLine();
                await boundChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await eventLoop.ShutdownGracefullyAsync();
            }
        }


    }
}
