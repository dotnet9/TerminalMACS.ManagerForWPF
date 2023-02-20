using HarmonyLib;
using HelloHook;
using System.Reflection;

var student = new Student();
Console.WriteLine(student.GetDetails("沙漠尽头的狼"));

var harmony = new Harmony("https://dotnet9.com");
harmony.PatchAll(Assembly.GetExecutingAssembly());

Console.WriteLine(student.GetDetails("沙漠之狐"));
Console.WriteLine(student.GetDetails("Dotnet"));

Console.ReadLine();