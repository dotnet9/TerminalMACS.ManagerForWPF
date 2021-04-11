using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractFactory._4;
using System.Configuration;

//引入反射，必须要写
using System.Reflection;

namespace AbstractFactory._5
{
  class DataAccess
  {
    // 程序集名称："抽象工厂模式"
    private static readonly string AssemblyName = "抽象工厂模式";

    // 数据库名称，可以替换成Access
    private static readonly string db = "sqlserver";

    public static IUser CreateUser()
    {
      string className = AssemblyName + "." + db + "User";
      return (IUser)Assembly.Load(AssemblyName).CreateInstance(className);
    }

    public static IDepartment CreateDepartment()
    {
      string className = AssemblyName + "." + db + "Department";
      return (IDepartment)Assembly.Load(AssemblyName).CreateInstance(className);
    }
  }
}
