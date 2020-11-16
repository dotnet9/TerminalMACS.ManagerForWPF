using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MessagePack;
using System;
using System.Collections.Generic;

namespace NettyModel.Coder
{
    public class MessagePackDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            byte[] array;
            int length = input.ReadableBytes;
            array = new byte[length];
            input.GetBytes(input.ReaderIndex, array, 0, length);

            output.Add(MessagePackSerializer.Deserialize<Object>(array));
            //output.Add(MessagePackSerializer.ToJson(array));
        }
    }
}
