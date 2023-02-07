using System.IO;
using System.Text;

namespace RAPTest2;

public class RedirectToOutputTextWriter : TextWriter
{
    private readonly MainWindow mainWindow;

    public RedirectToOutputTextWriter(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void WriteLine(string value)
    {
        mainWindow.OutputLine(value);
    }
}