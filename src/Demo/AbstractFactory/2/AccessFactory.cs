namespace AbstractFactory._2;

internal class AccessFactory : IFactory
{
    public IUser CreateUser()
    {
        return new AccessUser();
    }
}