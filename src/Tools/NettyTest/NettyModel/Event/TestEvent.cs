using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyModel.Event
{
    [MessagePackObject]// 使用MessagePack传输的实体类需要加入该注解
    public class TestEvent : BaseEvent
    {
        public TestEvent() { }
    }
}
