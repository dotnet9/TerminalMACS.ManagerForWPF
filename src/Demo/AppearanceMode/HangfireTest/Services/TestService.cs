using System;

namespace HangfireTest.Services;

public class TestService : ITestService
{
    public void WriteLine()
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
    }
}