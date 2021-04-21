using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMode._4
{
  public class Work
  {
    public State current { get; set; }

    /// <summary>
    /// 工作初始化为上午工作状态，即上午9点开始上班
    /// </summary>
    public Work()
    {
      current = new ForenoonState();
    }

    public int Hour { get; set; }

    public bool TaskFinished { get; set; }

    public void SetState(State state)
    {
      this.current = state;
    }

    public void WriteProgram()
    {
      current.WriteProgram(this);
    }
  }
}
