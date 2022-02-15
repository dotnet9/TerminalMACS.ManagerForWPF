using System.Reflection;
using AbstractFactory._4;
//引入反射，必须要写

namespace AbstractFactory._5;

internal class DataAccess
{
    // 程序集名称："抽象工厂模式"
    private static readonly string AssemblyName = "抽象工厂模式";

    // 数据库名称，可以替换成Access
    private static readonly string db = "sqlserver";

    public static IUser CreateUser()
    {
        var className = AssemblyName + "." + db + "User";
        return (IUser)Assembly.Load(AssemblyName).CreateInstance(className);
    }

    public static IDepartment CreateDepartment()
    {
        var className = AssemblyName + "." + db + "Department";
        return (IDepartment)Assembly.Load(AssemblyName).CreateInstance(className);
    }
}