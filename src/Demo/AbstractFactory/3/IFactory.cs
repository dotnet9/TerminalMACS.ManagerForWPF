using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory._3
{
  interface IFactory
  {
    IUser CreateUser();
    // 增加的接口方法
    IDepartment CreateDepartment();
  }
}
