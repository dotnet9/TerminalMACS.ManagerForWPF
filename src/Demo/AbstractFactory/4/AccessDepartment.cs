using System;

namespace AbstractFactory._4;

internal class AccessDepartment : IDepartment
{
    public void Insert(Department department)
    {
        Console.WriteLine("在Access中给Department表增加一条记录");
    }

    public Department GetDepartment(int id)
    {
        Console.WriteLine("在Access中根据ID得到Department表一条记录");
        return null;
    }
}