using System;
using Vanara.PInvoke;

namespace Activities.Shared.Utils;

public static class Common
{
    public static void OpenConsole()
    {
        Kernel32.AllocConsole();
    }

    public static void CloseConsole()
    {
        Kernel32.FreeConsole();
    }
}
