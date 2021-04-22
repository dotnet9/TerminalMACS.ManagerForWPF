using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterMode._1
{
  class Adaptee
  {
    public void SpecificRequest()
    {
      Console.WriteLine("特殊请求");
    }
  }
}
