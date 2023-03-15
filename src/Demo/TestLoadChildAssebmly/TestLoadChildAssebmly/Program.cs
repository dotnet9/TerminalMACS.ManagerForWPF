using System.Reflection;

var childAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChildAssembly.dll");
if (File.Exists(childAssemblyPath))
{
    var childAssembly = Assembly.LoadFile(childAssemblyPath);
    var childAssemblyName = childAssembly.GetName();
}

var studentType = Type.GetType("ChildAssembly.Student");
if (studentType != null)
{
    Console.WriteLine($"Student类型全称：{studentType.FullName}");
}