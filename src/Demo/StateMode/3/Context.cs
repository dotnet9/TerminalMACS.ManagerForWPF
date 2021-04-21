using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMode._3
{
  class Context
  {
    private State state;
    public State State
    {
      get { return state; }
      set
      {
        this.state = value;
        Console.WriteLine($"当前状态：{state.GetType().Name}");
      }
    }

    /// <summary>
    /// 定义Context的初始状态
    /// </summary>
    /// <param name="state"></param>
    public Context(State state)
    {
      this.State = state;
    }

    /// <summary>
    /// 对请求做处理，并设置下一状态
    /// </summary>
    public void Request()
    {
      State.Handle(this);
    }
  }
}
