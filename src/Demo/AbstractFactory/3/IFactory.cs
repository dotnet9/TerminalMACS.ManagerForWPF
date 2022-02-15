namespace AbstractFactory._3;

internal interface IFactory
{
    IUser CreateUser();

    // 增加的接口方法
    IDepartment CreateDepartment();
}