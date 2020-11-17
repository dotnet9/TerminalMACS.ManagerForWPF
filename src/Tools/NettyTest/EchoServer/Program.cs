using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
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
        static NettyServerHandler nettyServerHandler = new NettyServerHandler();


        static async Task RunServerAsync()
        {
            // 主工作线程组，设置为1个线程
            IEventLoopGroup bossGroup =new MultithreadEventLoopGroup(1);
            // 工作线程组，默认为内核数*2的线程数
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            try
            {
                //声明一个服务端Bootstrap，每个Netty服务端程序，都由ServerBootstrap控制，
                //通过链式的方式组装需要的参数
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);                                    // 设置主和工作线程组
                bootstrap.Channel<TcpServerSocketChannel>();                                // 设置通道模式为TcpSocket
                bootstrap.ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                    //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                    IChannelPipeline pipeline = channel.Pipeline;

                    //配置编码解码器
                    pipeline.AddLast("frameDecoder", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                    pipeline.AddLast("msgPackDecoder", new MessagePackDecoder());
                    pipeline.AddLast("frameEncoder", new LengthFieldPrepender(4));
                    pipeline.AddLast("msgPackEncoder", new MessagePackEncoder());

                    // IdleStateHandler 心跳
                    //服务端为读IDLE
                    pipeline.AddLast(new IdleStateHandler(150, 0, 0));      //第一个参数为读，第二个为写，第三个为读写全部
                    
                    //业务handler ，这里是实际处理业务的Handler
                    pipeline.AddLast("handler", nettyServerHandler);
                }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                IChannel boundChannel = await bootstrap.BindAsync(10086);
                ThreadPool.QueueUserWorkItem(sen =>
                {
                    while (true)
                    {
                        if (nettyServerHandler != null)
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
                
                //关闭服务
                await boundChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                //释放工作组线程
                await Task.WhenAll(bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                                   workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }


    }
}
