namespace AbstractFactory._3;

internal interface IDepartment
{
    void Insert(Department department);
    Department GetDepartment(int id);
}