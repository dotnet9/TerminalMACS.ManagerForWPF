using System.Configuration;

namespace TerminalMACS.Infrastructure.UI;

public class ConfigHelper
{
    public static string ReadKey(string key)
    {
        var cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        return cfa.AppSettings.Settings[key].Value;
    }

    public static void SetKey(string key, string value = "")
    {
        var cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        cfa.AppSettings.Settings[key].Value = value;
        cfa.Save();
    }
}