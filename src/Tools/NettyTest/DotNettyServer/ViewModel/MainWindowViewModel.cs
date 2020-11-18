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
        private bool _IsStartServerButtonEnabled = true;
        /// <summary>
        /// 开启服务按钮是否可用
        /// </summary>

        public bool IsStartServerButtonEnabled
        {
            get { return _IsStartServerButtonEnabled; }
            set { SetProperty(ref _IsStartServerButtonEnabled, value); }
        }
        private string _StartServerButtonContent = "开启服务";
        /// <summary>
        /// 开启、关闭服务按钮显示内容
        /// </summary>

        public string StartServerButtonContent
        {
            get { return _StartServerButtonContent; }
            set { SetProperty(ref _StartServerButtonContent, value); }
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
        public NettyServerHandler DotNettyServerHandler { get; private set; } = new NettyServerHandler();

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
            IsStartServerButtonEnabled = false;

            // 主工作线程组，设置为1个线程
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
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
                    pipeline.AddLast("handler", DotNettyServerHandler);
                }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                IChannel boundChannel = await bootstrap.BindAsync(ServerPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                //释放工作组线程
                //await Task.WhenAll(bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                //                 workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
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

                DotNettyServerHandler.SendData(new TestEvent()
                {
                    code = EventCode.Chat,
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
