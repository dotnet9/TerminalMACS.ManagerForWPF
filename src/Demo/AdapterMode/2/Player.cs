using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterMode._2
{
  /// <summary>
  /// 球员
  /// </summary>
  abstract class Player
  {
    protected string name;
    public Player(string name)
    {
      this.name = name;
    }

    // 进攻和防守方法
    public abstract void Attack();
    public abstract void Defense();
  }
}
