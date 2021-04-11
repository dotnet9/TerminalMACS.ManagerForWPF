using System;

namespace AbstractFactory._4
{
  class Program
  {
    static void Main(string[] args)
    {
      User user = new User();
      Department dept = new Department();

      // 直接得到实际的数据库访问实例，而不存在任何依赖
      IUser iu = DataAccess.CreateUser();

      iu.Insert(user);
      iu.GetUser(1);

      // 直接得到实际的数据库访问实例，而不存在任何依赖
      IDepartment id = DataAccess.CreateDepartment();

      id.Insert(dept);
      id.GetDepartment(1);

      Console.Read();
    }
  }
}
