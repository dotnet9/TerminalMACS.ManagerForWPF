using System.Reflection;
using System.Windows;

namespace WpfAppForZoomInAndZoomOut;

public partial class App : Application
{
    public App()
    {
        TestLoadLibrary();
    }

    private void TestLoadLibrary()
    {
        var assembly = Assembly.LoadFrom("TestDynamicLoadClassLibrary.dll");
        //var type = assembly.GetType("TestDynamicLoadClassLibrary.Test");
        var type = Type.GetType("TestDynamicLoadClassLibrary.Test, TestDynamicLoadClassLibrary, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null");
        Console.WriteLine(type.FullName);
    }
}