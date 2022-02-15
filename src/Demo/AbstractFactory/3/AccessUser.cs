using System;

namespace AbstractFactory._3;

internal class AccessUser : IUser
{
    public void Insert(User user)
    {
        Console.WriteLine("在Access中给User表增加一条记录");
    }

    public User GetUser(int id)
    {
        Console.WriteLine("在Access中根据ID得到Uuser表一条记录");
        return null;
    }
}