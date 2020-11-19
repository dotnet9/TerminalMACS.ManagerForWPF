using MessagePack;
using NettyModel.Event;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace NettyModel.Event
{
    public class NettyBodyCounter
    {
        public NettyBody NettyBody { get; set; }
        public int TryCount { get; set; }
    }

    /// <summary>
    /// 消息体
    /// </summary>
    [MessagePackObject]// 使用MessagePack传输的实体类需要加入该注解
    public class NettyBody
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        [Key(0)]
        public int code { get; set; }

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

        /**
     * 需要有默认构造
     * 否则会出现错误：Caused by: compile error: no such constructor:
     */
        public NettyBody()
        {

        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public NettyBody(NettyCodeEnum codeEnum)
        {
            this.code = (int)codeEnum;
            this.msg = codeEnum.ToString();
            this.reqId = Guid.NewGuid().ToString();
            this.time = UtilHelper.GetCurrentTimeStamp();
        }

        public NettyBody(NettyCodeEnum codeEnum, string reqId)
        {
            this.code = (int)codeEnum;
            this.msg = codeEnum.ToString();
            this.reqId = reqId;
            this.time = UtilHelper.GetCurrentTimeStamp();
        }

        public NettyBody(int code, String msg)
        {
            this.code = code;
            this.msg = msg;
            this.reqId = Guid.NewGuid().ToString();
            this.time = UtilHelper.GetCurrentTimeStamp();
        }

        public NettyBody(int code, String msg, String reqId)
        {
            this.code = code;
            this.msg = msg;
            this.reqId = reqId;
            this.time = UtilHelper.GetCurrentTimeStamp();
        }

        public static NettyBody success()
        {
            return new NettyBody(NettyCodeEnum.OK);
        }

        public static NettyBody success(String reqId)
        {
            return new NettyBody(NettyCodeEnum.OK, reqId);
        }

        public static NettyBody error()
        {
            return new NettyBody(NettyCodeEnum.ServerBusy);
        }

        public static NettyBody ping(String reqId)
        {
            return new NettyBody(NettyCodeEnum.Ping, reqId);
        }

        public static NettyBody ping()
        {
            return new NettyBody(NettyCodeEnum.Ping);
        }

    }
}
