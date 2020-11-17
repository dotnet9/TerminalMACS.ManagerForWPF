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
        private static readonly UptimeServerHandler handler = new UptimeServerHandler();


        static async Task RunServerAsync()
        {
            IEventLoopGroup bossGroup=new MultithreadEventLoopGroup(1);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            try
            {
                // 服务器引导程序
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);
                bootstrap.Channel<TcpServerSocketChannel>();
                bootstrap.ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("frameDecoder", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                    pipeline.AddLast("msgPackDecoder", new MessagePackDecoder());
                    pipeline.AddLast("frameEncoder", new LengthFieldPrepender(4));
                    pipeline.AddLast("msgPackEncoder", new MessagePackEncoder());
                    nettyServerHandler = new NettyServerHandler();
                    pipeline.AddLast("handler", nettyServerHandler);
                    pipeline.AddLast("Uptime", handler);
                }));
                IChannel boundChannel = await bootstrap.BindAsync(10086);
                ThreadPool.QueueUserWorkItem(sen =>
                {
                    while (true)
                    {
                        if (nettyServerHandler != null && nettyServerHandler.Socket != null && nettyServerHandler.Socket.Channel.Active)
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
                await Task.WhenAll(bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                                   workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }


    }
}
