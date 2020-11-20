using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNettyServer.DotNetty;
using DotNettyServer.Models;
using HandyControl.Data;
using NettyModel;
using NettyModel.Coder;
using NettyModel.Event;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DotNettyServer.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; } = new ObservableCollection<ChatInfoModel>();

        private int _ServerPort = 10086;
        /// <summary>
        /// 服务端端口
        /// </summary>

        public int ServerPort
        {
            get { return _ServerPort; }
            set { SetProperty(ref _ServerPort, value); }
        }

        private bool _StartServerButtonEnabled = true;
        /// <summary>
        /// 开启、关闭服务按钮可用状态
        /// </summary>

        public bool StartServerButtonEnabled
        {
            get { return _StartServerButtonEnabled; }
            set { SetProperty(ref _StartServerButtonEnabled, value); }
        }

        /// <summary>
        /// 待发送的聊天内容
        /// </summary>
        private string _ChatString;

        public string ChatString
        {
            get { return _ChatString; }
            set { SetProperty(ref _ChatString, value); }
        }

        public ICommand RaiseStartServerCommand { get; private set; }

        public ICommand RaiseSendStringCommand { get; private set; }

        /// <summary>
        /// DotNetty处理程序
        /// </summary>
        public NettyServerChannelHandler DotNettyServerHandler { get; private set; } = new NettyServerChannelHandler();

        private readonly string _id = Guid.NewGuid().ToString();

        public MainWindowViewModel()
        {
            RaiseStartServerCommand = new DelegateCommand(RaiseStartServerHandler);
            RaiseSendStringCommand = new DelegateCommand(RaiseSendStringHandler);
            DotNettyServerHandler.ReceiveEventFromClientEvent += ReceiveMessage;
        }

        /// <summary>
        /// 开启、关闭DotNetty服务
        /// </summary>
        private async void RaiseStartServerHandler()
        {
            StartServerButtonEnabled = false;

            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);
                bootstrap.Channel<TcpServerSocketChannel>();
                bootstrap.ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                    pipeline.AddLast(new MessagePackDecoder());
                    pipeline.AddLast(new LengthFieldPrepender(4));
                    pipeline.AddLast(new MessagePackEncoder());
                    pipeline.AddLast(DotNettyServerHandler);
                }));

                await bootstrap.BindAsync(ServerPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接服务异常：{ex.Message}");
            }
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
            if (DotNettyServerHandler != null)
            {
                DotNettyServerHandler.SendData(new NettyBody()
                {
                    code = (int)NettyCodeEnum.Chat,
                    time = UtilHelper.GetCurrentTimeStamp(),
                    msg = "服务器推送",
                    fromId = "",
                    reqId = Guid.NewGuid().ToString(),
                    data = ChatString
                });

            }
            ChatString = string.Empty;
        }

        /// <summary>
        /// 收到信息
        /// </summary>
        /// <param name="testEvent"></param>
        private void ReceiveMessage(NettyBody testEvent)
        {
            if(App.Current==null)
            {
                return;
            }
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
