﻿namespace HookDemos.OwnerLibrary;

public class Student
{
    public string GetDetails(string name)
    {
        return $"大家好，我是Dotnet9网站站长：{name}";
    }

    public override string ToString()
    {
        return nameof(Student);
    }
}