using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory._2
{
  class AccessFactory : IFactory
  {
    public IUser CreateUser()
    {
      return new AccessUser();
    }
  }
}
