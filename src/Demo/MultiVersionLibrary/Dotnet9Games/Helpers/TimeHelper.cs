namespace Dotnet9Games.Helpers;

internal class TimeHelper
{
    internal static string FormatSeconds(double totalSeconds)
    {
        var seconds = (int)(totalSeconds % 60);
        var minutes = (int)((totalSeconds / 60) % 60);
        var hours = (int)((totalSeconds / 60 / 60) % 24);
        var days = (int)(totalSeconds / 60 / 60 / 24);

        string formattedTime = "";

        if (days > 0)
        {
            formattedTime += $"{days}天";
        }

        if (hours > 0)
        {
            formattedTime += $"{hours}小时";
        }

        if (minutes > 0)
        {
            formattedTime += $"{minutes}分";
        }

        formattedTime += $"{seconds}秒";

        return formattedTime;
    }
}