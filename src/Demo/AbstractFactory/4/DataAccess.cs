namespace AbstractFactory._4;

internal class DataAccess
{
    // 数据库名称，可替换成Access
    private static readonly string db = "Sqlserver";
    //private static readonly string db = "Access" ;

    public static IUser CreateUser()
    {
        IUser result = null;


        // 由于db的事先设置，所以此处可以根据选择实例化出相应的对象
        switch (db)

        {
            case "Sqlserver":
                result = new SqlserverUser();
                break;
            case "Access":
                result = new AccessUser();
                break;
        }

        return result;
    }

    public static IDepartment CreateDepartment()
    {
        IDepartment result = null;
        switch (db)
        {
            case "Sqlserver":
                result = new SqlserverDepartment();
                break;
            case "Access":
                result = new AccessDepartment();
                break;
        }

        return result;
    }
}