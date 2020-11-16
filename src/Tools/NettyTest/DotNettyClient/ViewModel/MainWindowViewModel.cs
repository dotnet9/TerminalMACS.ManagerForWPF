using DotNettyClient.Models;
using HandyControl.Data;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DotNettyClient.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; } = new ObservableCollection<ChatInfoModel>();
        private string _ChatString;

        public string ChatString
        {
            get { return _ChatString; }
            set { SetProperty(ref _ChatString, value); }
        }
        public ICommand RaiseSendStringCommand;


        private readonly string _id = Guid.NewGuid().ToString();

        public MainWindowViewModel()
        {
            RaiseSendStringCommand = new DelegateCommand(RaiseSendStringHandler);
        }

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
            ChatString = string.Empty;
        }

        private void ReceiveMessage(ChatInfoModel info)
        {
            info.Role = ChatRoleType.Receiver;
            ChatInfos.Add(info);
        }
    }
}
