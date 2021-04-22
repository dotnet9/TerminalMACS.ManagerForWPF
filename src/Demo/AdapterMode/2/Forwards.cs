using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterMode._2
{
  /// <summary>
  /// 前锋
  /// </summary>
  class Forwards : Player
  {
    public Forwards(string name) : base(name)
    {

    }
    public override void Attack()
    {
      Console.WriteLine($"前锋 {name} 进攻");
    }

    public override void Defense()
    {
      Console.WriteLine($"前锋 {name} 防守");
    }
  }

  /// <summary>
  /// 中锋
  /// </summary>
  class Center : Player
  {
    public Center(string name) : base(name)
    {

    }
    public override void Attack()
    {
      Console.WriteLine($"中锋 {name} 进攻");
    }

    public override void Defense()
    {
      Console.WriteLine($"中锋 {name} 防守");
    }
  }

  /// <summary>
  /// 后卫
  /// </summary>
  class Guards : Player
  {
    public Guards(string name) : base(name)
    {

    }
    public override void Attack()
    {
      Console.WriteLine($"后卫 {name} 进攻");
    }

    public override void Defense()
    {
      Console.WriteLine($"后卫 {name} 防守");
    }
  }

  /// <summary>
  /// 外籍中锋
  /// </summary>
  class ForeignCenter
  {
    /// <summary>
    /// 外籍中锋类球员的姓名故意用属性而不是构造方法来区别与前三个球员类的不同
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 表明‘外籍中锋’只懂得中文‘进攻’
    /// </summary>
    public void 进攻()
    {
      Console.WriteLine($"外籍中锋 {Name} 进攻");
    }

    /// <summary>
    /// 表明‘外籍中锋’只懂得中文‘防守’
    /// </summary>
    public void 防守()
    {
      Console.WriteLine($"外籍中锋 {Name} 防守");
    }
  }
}
