namespace SocketCore.Utils;

public static class TimestampHelper
{
    public static long ToTimestamp(this DateTime dateTime)
    {
        var time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var ts = dateTime - time;
        return (long)ts.TotalMilliseconds;
    }

    public static long GetTimestamp()
    {
        return ToTimestamp(DateTime.Now);
    }

    public static DateTime ToDateTime(this long milliseconds)
    {
        var time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var dt = time.AddMilliseconds(milliseconds);
        return dt;
    }
}