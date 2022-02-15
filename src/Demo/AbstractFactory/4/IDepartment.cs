namespace AbstractFactory._4;

internal interface IDepartment
{
    void Insert(Department department);
    Department GetDepartment(int id);
}