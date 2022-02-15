using System.Diagnostics;

namespace TerminalMACS.Infrastructure;

public class FileHelper
{
    public static void OpenFolderAndSelectFile(string fileFullName)
    {
        var psi = new ProcessStartInfo("Explorer.exe");
        psi.Arguments = "/e,/select," + fileFullName;
        Process.Start(psi);
    }
}