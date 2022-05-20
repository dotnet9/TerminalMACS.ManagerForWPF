using System.Windows;

namespace SimpleGuide;

public class GuidVo
{
    private FrameworkElement uc;
    private string content;

    public FrameworkElement Uc
    {
        get
        {
            return uc;
        }

        set
        {
            uc = value;
        }
    }

    public string Content
    {
        get
        {
            return content;
        }

        set
        {
            content = value;
        }
    }
}