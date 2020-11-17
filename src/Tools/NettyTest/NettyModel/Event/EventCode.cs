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
        AnalysisFail = 10010001,    // 数据解析失败
        SaveFail = 10010002,        // 数据存储失败
        FuBin = 10010003,           // 覆冰数据
        Ping = 10010004,            // 心跳
        ServerBusy = 500            // 服务繁忙
    }
}
