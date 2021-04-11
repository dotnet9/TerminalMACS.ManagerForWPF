using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory._4
{
  interface IUser
  {
    void Insert(User user);
    User GetUser(int id);
  }
}
