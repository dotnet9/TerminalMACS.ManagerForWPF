using System;

namespace AbstractFactory._4;

internal class Program
{
    private static void Main(string[] args)
    {
        var user = new User();
        var dept = new Department();

        // 直接得到实际的数据库访问实例，而不存在任何依赖
        var iu = DataAccess.CreateUser();

        iu.Insert(user);
        iu.GetUser(1);

        // 直接得到实际的数据库访问实例，而不存在任何依赖
        var id = DataAccess.CreateDepartment();

        id.Insert(dept);
        id.GetDepartment(1);

        Console.Read();
    }
}