using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyModel.Coder;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DotNettyClient.DotNetty
{
    public class NettyClient : IDisposable
    {
        private static DateTime dtLastConnectTime = default(DateTime);
        private bool isDisposed = false;
        private string serverIP;
        private int serverPort;
        public NettyClient(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
        }
        public async Task ConnectServer()
        {
            if ((DateTime.Now - dtLastConnectTime).TotalSeconds <= 5)
            {
                return;
            }
            dtLastConnectTime = DateTime.Now;

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
                var waitResult = bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIP), serverPort)).Wait(TimeSpan.FromSeconds(15));
                if (waitResult)
                {
                    ClientEventHandler.RecordLogEvent?.Invoke("连接服务成功");
                }
                else
                {
                    ClientEventHandler.RecordLogEvent?.Invoke("尝试连接服务失败，请检查服务端状态");
                    ClientEventHandler.IsConnect = false;
                }
            }
            catch (Exception ex)
            {
                ClientEventHandler.RecordLogEvent?.Invoke($"尝试连接服务失败，请检查服务端状态： {ex.Message}");
                ClientEventHandler.IsConnect = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // 执行资源释放操作
                }
                isDisposed = true;
            }
        }
    }
}
