using System.IO;
using System.Text;

namespace RAPTest2;

public class RedirectToOutputTextWriter : TextWriter
{
    private MainWindow mainWindow;

    public RedirectToOutputTextWriter(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    public override Encoding Encoding
    {
        get { return Encoding.UTF8; }
    }

    public override void WriteLine(string value)
    {
        this.mainWindow.OutputLine(value);
    }
}