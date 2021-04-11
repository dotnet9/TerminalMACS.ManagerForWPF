using System;

namespace AbstractFactory._3
{
  class SqlserverUser : IUser
  {
    public void Insert(User user)
    {
      Console.WriteLine("在SQL Server中给User表增加一条记录");
    }

    public User GetUser(int id)
    {
      Console.WriteLine("在SQL Server中根据ID得到Uuser表一条记录");
      return null;
    }
  }
}
