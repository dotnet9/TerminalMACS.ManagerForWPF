
namespace AbstractFactory._2
{
  class SqlServerFactory : IFactory
  {
    public IUser CreateUser()
    {
      return new SqlserverUser();
    }
  }
}
