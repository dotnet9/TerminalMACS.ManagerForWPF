using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory._3
{
  class AccessFactory : IFactory
  {
    public IUser CreateUser()
    {
      return new AccessUser();
    }

    // 增加了OleDBDepartment工厂
    public IDepartment CreateDepartment()
    {
      return new AccessDepartment();
    }

  }
}
