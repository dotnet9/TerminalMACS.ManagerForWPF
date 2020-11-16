using MessagePack;
using NettyModel.Event;
using System;

namespace NettyModel.Event
{

    [MessagePackObject]// 使用MessagePack传输的实体类需要加入该注解
    public abstract class BaseEvent
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        [Key(0)]
        public EventCode code { get; set; }

        /// <summary>
        /// 发送时间-时间戳
        /// </summary>
        [Key(1)]
        public long time { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        [Key(2)]
        public string msg { get; set; }

        /// <summary>
        /// 发送者ID
        /// </summary>
        [Key(3)]
        public string fromId { get; set; }

        /// <summary>
        /// 消息唯一ID
        /// </summary>
        [Key(4)]
        public string reqId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Key(5)]
        public string data { get; set; }
    }
}
