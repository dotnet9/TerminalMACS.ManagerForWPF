using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMode._3
{
  class ConcreteStateA : State
  {
    /// <summary>
    /// 设置ConcreteStateA的下一状态是 ConcreteStateB
    /// </summary>
    /// <param name="context"></param>
    public override void Handle(Context context)
    {
      context.State = new ConcreteStateB();
    }
  }

  class ConcreteStateB : State
  {
    /// <summary>
    /// 设置ConcreteStateB的下一状态是 ConcreteStateA
    /// </summary>
    /// <param name="context"></param>
    public override void Handle(Context context)
    {
      context.State = new ConcreteStateA();
    }
  }
}
