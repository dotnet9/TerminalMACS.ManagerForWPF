using System;

namespace AbstractFactory._1;

internal class SqlserverUser
{
    public void Insert(User user)
    {
        Console.WriteLine("在 sQL server中给User表增加一条记录");
    }

    public User GetUser(int id)
    {
        Console.WriteLine("在sQL server中根据ID得到User表一条记录");
        return null;
    }
}