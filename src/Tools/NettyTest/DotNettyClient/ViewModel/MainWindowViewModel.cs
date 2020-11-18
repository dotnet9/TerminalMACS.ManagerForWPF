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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DotNettyClient.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<ChatInfoModel> _ChatInfo = new ObservableCollection<ChatInfoModel>();
        public ObservableCollection<ChatInfoModel> ChatInfos { get { return _ChatInfo; } }
        private string _ServerIP = "192.168.50.87";//"127.0.0.1";//
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
        private bool isConnectSuccess = false;
        private bool _IsConnectServer = false;
        /// <summary>
        /// 开启服务按钮是否可用
        /// </summary>

        public bool IsConnectServer
        {
            get { return _IsConnectServer; }
            set { SetProperty(ref _IsConnectServer, value); }
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

        public event Action<string> RecordLogEvent;

        /// <summary>
        /// 客户端处理程序
        /// </summary>
        public EchoClientHandler DotNettyClientHandler { get; private set; }
        private readonly string _id = Guid.NewGuid().ToString();

        public MainWindowViewModel()
        {
            RaiseConnectServerCommand = new DelegateCommand(RaiseConnectServerHandler);
            RaiseSendStringCommand = new DelegateCommand(RaiseSendStringHandler);
            InitDotNetty();
        }

        MultithreadEventLoopGroup group = null;
        Bootstrap bootstrap = null;

        private void InitDotNetty()
        {
            group = new MultithreadEventLoopGroup();
            bootstrap = new Bootstrap();
            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(3))
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
                    pipeline.AddLast(new IdleStateHandler(0, 0, 0));

                    // 消息处理handler
                    if (DotNettyClientHandler != null)
                    {
                        DotNettyClientHandler.ReceiveEventFromClientEvent -= ReceiveMessage;
                        DotNettyClientHandler.ReconnectServer -= () => ConnectToServer().Wait();
                        DotNettyClientHandler.RecordLogEvent -= this.RecordLogEvent;
                    }
                    DotNettyClientHandler = new EchoClientHandler();
                    DotNettyClientHandler.ReceiveEventFromClientEvent += ReceiveMessage;
                    DotNettyClientHandler.ReconnectServer += () => ConnectToServer().Wait();
                    DotNettyClientHandler.RecordLogEvent += this.RecordLogEvent;
                    pipeline.AddLast("handler", DotNettyClientHandler);
                }));
            bootstrap.RemoteAddress(new IPEndPoint(IPAddress.Parse(this.ServerIP), this.ServerPort));
            CheckConnectServer();
        }

        private bool isCheckConnect = false;
        private void CheckConnectServer()
        {
            if (isCheckConnect)
            {
                return;
            }
            isCheckConnect = true;

            ThreadPool.QueueUserWorkItem(sen =>
            {
                while (true)
                {
                    if (!isConnectSuccess)
                    {
                        ConnectToServer();
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            });
        }

        /// <summary>
        /// 连接DotNetty服务端
        /// </summary>
        private async void RaiseConnectServerHandler()
        {
            IsConnectServer = true;
            ConnectServerButtonContent = "重连服务";
            if (!IsConnectServer)
            {
                if (this.group != null)
                {
                    this.DotNettyClientHandler.Disconnect();
                }
                return;
            }
            else
            {
                await ConnectToServer();
            }
        }

        public async Task ConnectToServer()
        {
            try
            {
                IChannel clientChannel = null;
                try
                {
                    isConnectSuccess = false;
                    clientChannel = await bootstrap.ConnectAsync();
                    isConnectSuccess = true;
                }
                catch (Exception ex)
                {
                    isConnectSuccess = false;
                }
            }
            finally
            {
                Console.ReadLine();
                //await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        private async Task RunClientAsync()
        {
            await ConnectToServer();
        }

        /// <summary>
        /// 发送聊天内容
        /// </summary>
        private void RaiseSendStringHandler()
        {

            if (string.IsNullOrEmpty(ChatString)) return;
            App.Current.Dispatcher.Invoke(() =>
            {
                var info = new ChatInfoModel
                {
                    Message = ChatString,
                    SenderId = _id,
                    Type = ChatMessageType.String,
                    Role = ChatRoleType.Sender
                };
                ChatInfos.Add(info);
            });
            if (DotNettyClientHandler != null)
            {
                DotNettyClientHandler.SendData(new TestEvent()
                {
                    code = EventCode.Chat,
                    time = UtilHelper.GetCurrentTimeStamp(),
                    msg = "客户端请求",
                    fromId = "",
                    reqId = Guid.NewGuid().ToString(),
                    data = ChatString
                });
            }
            ChatString = string.Empty;
        }

        private void ReceiveMessage(TestEvent testEvent)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ChatInfoModel info = new ChatInfoModel
                {

                    Message = testEvent.data,
                    SenderId = "ddd",
                    Type = ChatMessageType.String,
                    Role = ChatRoleType.Receiver
                };
                ChatInfos.Add(info);
            });
        }
    }
}
