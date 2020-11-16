using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunClientAsync().Wait();
        }

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
                        pipeline.AddLast(new EchoClientHandler());
                    }));
                IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.50.87"), 10010));
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
    }
}
