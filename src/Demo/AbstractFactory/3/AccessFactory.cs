namespace AbstractFactory._3;

internal class AccessFactory : IFactory
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