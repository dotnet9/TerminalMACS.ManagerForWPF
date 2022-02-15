using System;

namespace TerminalMACS.Infrastructure.Services;

public class TestService : ITestService
{
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
    }
}