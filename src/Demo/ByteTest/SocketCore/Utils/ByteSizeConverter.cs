namespace SocketCore.Utils;

public static class ByteSizeConverter
{
    public static string FormatByte(long byteSize)
    {
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        return Format(byteSize, units);
    }

    public static string FormatMB(long byteSize)
    {
        string[] units = { "MB", "GB", "TB" };
        return Format(byteSize, units);
    }

    public static string FormatGB(long byteSize)
    {
        string[] units = { "GB", "TB" };
        return Format(byteSize, units);
    }

    public static string Format(long byteSize, string[] units)
    {
        double totalSize = byteSize;
        int index = 0;
        while (totalSize > 1024 && index < (units.Length - 1))
        {
            totalSize /= 1024;
            index++;
        }

        string formattedSize = totalSize.ToString("F2");

        return formattedSize + units[index];
    }
}