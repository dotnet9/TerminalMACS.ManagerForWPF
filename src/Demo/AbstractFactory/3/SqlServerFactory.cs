using System;

namespace AbstractFactory._3;

internal class SqlServerFactory : IFactory
{
    public IUser CreateUser()
    {
        return new SqlserverUser();
    }

    // 增加了SqlserverDepartment工厂
    public IDepartment CreateDepartment()
    {
        throw new NotImplementedException();
    }
}