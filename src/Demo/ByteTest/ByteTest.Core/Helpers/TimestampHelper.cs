namespace ByteTest.Core.Helpers;

internal class TimestampHelper
{
    public static long GetTimestamp(DateTime dateTime)
    {
        var time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var ts = dateTime - time;
        return (long)ts.TotalMilliseconds;
    }

    public static DateTime GetDateTime(long milliseconds)
    {
        var time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var dt = time.AddMilliseconds(milliseconds);
        return dt;
    }
}