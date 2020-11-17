using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNettyClient.DotNetty;
using DotNettyClient.Models;
using HandyControl.Data;
using NettyModel;
using NettyModel.Coder;
using NettyModel.Event;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Input;

namespace DotNettyClient.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; } = new ObservableCollection<ChatInfoModel>();
        private string _ServerIP = "127.0.0.1";
        /// <summary>
        /// 服务端端口
        /// </summary>

        public string ServerIP
        {
            get { return _ServerIP; }
            set { SetProperty(ref _ServerIP, value); }
        }
        private int _ServerPort = 10086;
        /// <summary>
        /// 服务端端口
        /// </summary>

        public int ServerPort
        {
            get { return _ServerPort; }
            set { SetProperty(ref _ServerPort, value); }
        }
        private bool _IsConnectServerButtonEnabled = true;
        /// <summary>
        /// 开启服务按钮是否可用
        /// </summary>

        public bool IsConnectServerButtonEnabled
        {
            get { return _IsConnectServerButtonEnabled; }
            set { SetProperty(ref _IsConnectServerButtonEnabled, value); }
        }
        private string _ConnectServerButtonContent = "连接服务";
        /// <summary>
        /// 连接、关闭服务按钮显示内容
        /// </summary>

        public string ConnectServerButtonContent
        {
            get { return _ConnectServerButtonContent; }
            set { SetProperty(ref _ConnectServerButtonContent, value); }
        }
        private string _ChatString;

        public string ChatString
        {
            get { return _ChatString; }
            set { SetProperty(ref _ChatString, value); }
        }
        public ICommand RaiseConnectServerCommand { get; private set; }
        public ICommand RaiseSendStringCommand { get; private set; }


        EchoClientHandler echoClientHandler = new EchoClientHandler();
        private readonly string _id = Guid.NewGuid().ToString();

        public MainWindowViewModel()
        {
            RaiseConnectServerCommand = new DelegateCommand(RaiseConnectServerHandler);
            RaiseSendStringCommand = new DelegateCommand(RaiseSendStringHandler);
            echoClientHandler.ReceiveEventFromClientEvent += ReceiveMessage;
        }

        /// <summary>
        /// 连接DotNetty服务端
        /// </summary>
        private async void RaiseConnectServerHandler()
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

                        // IdleStateHandler 心跳
                        //客户端为写IDLE
                        pipeline.AddLast(new IdleStateHandler(0, 0, 10));

                        // 消息处理handler
                        echoClientHandler.DisconnectServer += () => RaiseConnectServerHandler();
                        pipeline.AddLast("handler", echoClientHandler);
                    }));

                // 192.168.50.87
                //IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("192.168.50.87"), 10086));
                IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10086));

                await clientChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// 发送聊天内容
        /// </summary>
        private void RaiseSendStringHandler()
        {
            if (string.IsNullOrEmpty(ChatString)) return;
            var info = new ChatInfoModel
            {
                Message = ChatString,
                SenderId = _id,
                Type = ChatMessageType.String,
                Role = ChatRoleType.Sender
            };
            ChatInfos.Add(info); 
            if (echoClientHandler != null)
            {
                echoClientHandler.SendData(new TestEvent()
                {
                    code = EventCode.FuBin,
                    time = UtilHelper.GetCurrentTimeStamp(),
                    msg = "客户端请求",
                    fromId = "",
                    reqId = $"",
                    data = $"客户端时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
                });
            }
            ChatString = string.Empty;
        }

        private void ReceiveMessage(TestEvent testEvent)
        {
            ChatInfoModel info = new ChatInfoModel
            {

                Message = testEvent.data,
                SenderId = "ddd",
                Type = ChatMessageType.String,
                Role = ChatRoleType.Receiver
            };
            ChatInfos.Add(info);
        }
    }
}
