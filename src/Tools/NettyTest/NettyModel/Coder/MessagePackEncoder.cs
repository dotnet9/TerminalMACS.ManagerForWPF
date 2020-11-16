using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MessagePack;
using System;

namespace NettyModel.Coder
{
    public class MessagePackEncoder : MessageToByteEncoder<Object>
    {
        protected override void Encode(IChannelHandlerContext context, object message, IByteBuffer output)
        {
            byte[] temp = MessagePackSerializer.Serialize(message);
            output.WriteBytes(temp);
        }
    }
}
