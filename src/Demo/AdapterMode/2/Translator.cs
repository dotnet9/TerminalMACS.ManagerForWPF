using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterMode._2
{
  class Translator : Player
  {
    // 声明并实例化一个内部‘外籍中锋’对象，表明翻译者与外籍球员有关联
    private ForeignCenter wjzf = new ForeignCenter();

    public Translator(string name)
      : base(name)
    {
      wjzf.Name = name;
    }

    /// <summary>
    /// 翻译者将‘Attack’翻译为‘进攻’告诉外籍中锋
    /// </summary>
    public override void Attack()
    {
      wjzf.进攻();
    }

    /// <summary>
    /// 翻译者将‘Defense’翻译为‘防守’告诉外籍中锋
    /// </summary>
    public override void Defense()
    {
      wjzf.防守();
    }
  }
}
