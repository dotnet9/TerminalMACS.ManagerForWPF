using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyModel.Coder;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotNettyClient.DotNetty
{
    public class NettyClient
    {
        private string serverIP;
        private int serverPort;
        public NettyClient(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
        }
        public async Task ConnectServer()
        {
            var group = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                        .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3))
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                        pipeline.AddLast(new MessagePackDecoder());
                        pipeline.AddLast(new LengthFieldPrepender(4));
                        pipeline.AddLast(new MessagePackEncoder());
                        pipeline.AddLast(new IdleStateHandler(ClientEventHandler.PING_INTERVAL, 0, 0));
                        pipeline.AddLast(new NettyClientChannelHandler(serverIP, serverPort));
                    }));
                ClientEventHandler.RecordLogEvent?.Invoke("尝试连接服务");
                var waitResult = bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIP), serverPort)).Wait(TimeSpan.FromSeconds(5));
                if (waitResult)
                {
                    ClientEventHandler.RecordLogEvent?.Invoke("连接服务成功");
                }
                else
                {
                    ClientEventHandler.RecordLogEvent?.Invoke("连接服务失败，重新连接");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    await ConnectServer();
                }
            }
            catch (Exception ex)
            {
                ClientEventHandler.RecordLogEvent?.Invoke($"尝试连接服务失败，请检查服务端状态： {ex.Message}");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                await ConnectServer();
            }
        }
    }
}
