using System.IO;
using System;
using System.Runtime.InteropServices;

namespace CsharpConsoleFramework
{
    internal class Program
    {
        [DllImport("CppDll.dll")]
        public extern static int Add(int left, int right);
        
        static void Main(string[] args)
        {
            var cppDllFileName = "CppDll.dll";
            var cppDllSourcePath =
                Path.Combine(Environment.CurrentDirectory, Environment.Is64BitProcess ? "x64" : "x86", cppDllFileName);
            if (File.Exists(cppDllFileName))
            {
                File.Delete(cppDllFileName);
            }

            File.Copy(cppDllSourcePath, cppDllFileName);
            var result = Add(1, 2);
            Console.WriteLine($"调用C++ DLL返回结果：{result}");
            Console.ReadLine();
        }
    }
}
