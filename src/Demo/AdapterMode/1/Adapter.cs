using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterMode._1
{
  class Adapter : Target
  {
    /// <summary>
    /// 建立一个私有的Adaptee对象
    /// </summary>
    private Adaptee adaptee = new Adaptee();

    /// <summary>
    /// 这样就可以把表面上调用Request()方法变成实际调用SpecificRequest()
    /// </summary>
    public override void Request()
    {
      adaptee.SpecificRequest();
    }
  }
}
