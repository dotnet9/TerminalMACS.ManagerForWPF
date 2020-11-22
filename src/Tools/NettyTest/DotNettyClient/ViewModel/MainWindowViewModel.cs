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
        private string _ServerIP = "127.0.0.1";//"192.168.50.87";//"192.168.50.154";//"192.168.101.3";//

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

        private bool _ConnectServerButtonEnabled = true;
        /// <summary>
        /// 连接、关闭服务按钮是否可用
        /// </summary>
        public bool ConnectServerButtonEnabled
        {
            get { return _ConnectServerButtonEnabled; }
            set { SetProperty(ref _ConnectServerButtonEnabled, value); }
        }

        private string _ChatString;

        public string ChatString
        {
            get { return _ChatString; }
            set { SetProperty(ref _ChatString, value); }
        }

        public ICommand RaiseConnectServerCommand { get; private set; }

        public ICommand RaiseSendStringCommand { get; private set; }

        private readonly string _id = Guid.NewGuid().ToString();

        public MainWindowViewModel()
        {
            RaiseConnectServerCommand = new DelegateCommand(RaiseConnectServerHandler);
            RaiseSendStringCommand = new DelegateCommand(RaiseSendStringHandler);
            ClientEventHandler.ReceiveEventFromClientEvent += ReceiveMessage;
        }


        /// <summary>
        /// 连接DotNetty服务端
        /// </summary>
        private void RaiseConnectServerHandler()
        {
            ConnectServerButtonEnabled = false;

            ClientEventHandler.SetServerAddress(ServerIP, ServerPort);
            Task.Run(()=>new NettyClient(ServerIP, ServerPort).ConnectServer().Wait());
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
            ClientEventHandler.SendData(new NettyBody()
            {
                code = (int)NettyCodeEnum.Chat,
                time = UtilHelper.GetCurrentTimeStamp(),
                msg = "客户端请求",
                fromId = "",
                reqId = Guid.NewGuid().ToString(),
                data = ChatString
            });
            ChatString = string.Empty;
        }

        private void ReceiveMessage(NettyBody testEvent)
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
