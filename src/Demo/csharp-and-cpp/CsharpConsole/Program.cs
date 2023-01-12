using System.Runtime.InteropServices;

var cppDllFileName = "CppDll.dll";
var cppDllSourcePath =
    Path.Combine(Environment.CurrentDirectory, IsX64Process() ? "x64" : "x86", cppDllFileName);
if (File.Exists(cppDllFileName))
{
    File.Delete(cppDllFileName);
}

File.Copy(cppDllSourcePath, cppDllFileName);
var result = CppDllTest.Add(1, 2);
Console.WriteLine($"调用C++ DLL返回结果：{result}");
Console.ReadLine();

bool IsX64Process()
{
    return 8 == nint.Size
           || (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")));
}

public class CppDllTest
{
    [DllImport("CppDll.dll")]
    public extern static int Add(int left, int right);
}