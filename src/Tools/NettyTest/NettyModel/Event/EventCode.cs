using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyModel.Event
{
    public enum EventCode
    {
        OK = 200,                   // 发送成功
        Ping = 10010001,            // 心跳
        Chat = 10010002,            // 聊天
    }
}
