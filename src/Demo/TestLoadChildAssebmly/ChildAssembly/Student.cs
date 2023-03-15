namespace ChildAssembly;
public class Student
{
    public string? Name { get; set; }

    public string GetDetails()
    {
        return $"这是帅哥：{Name}";
    }
}
