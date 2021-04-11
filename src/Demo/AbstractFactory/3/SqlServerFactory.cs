
namespace AbstractFactory._3
{
  class SqlServerFactory : IFactory
  {
    public IUser CreateUser()
    {
      return new SqlserverUser();
    }

    // 增加了SqlserverDepartment工厂
    public IDepartment CreateDepartment()
    {
      throw new System.NotImplementedException();
    }

  }
}
