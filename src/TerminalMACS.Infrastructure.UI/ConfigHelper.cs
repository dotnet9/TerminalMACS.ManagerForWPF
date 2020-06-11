using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TerminalMACS.Infrastructure.UI
{
    public class ConfigHelper
    {
        public static string ReadKey(string key)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return cfa.AppSettings.Settings[key].Value.ToString();
        }

        public static void SetKey(string key, string value = "")
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
    }
}
