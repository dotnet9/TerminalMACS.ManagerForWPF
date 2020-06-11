using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using WpfExtensions.Xaml;

namespace TerminalMACS.Infrastructure.UI
{
    public class LanguageHelper
    {
        private static string _lastLanguage = "";
        private const string KEY_OF_LANGUAGE = "language";
        public static void SetLanguage(string language = "")
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = ConfigHelper.ReadKey(KEY_OF_LANGUAGE);
                if (string.IsNullOrWhiteSpace(language))
                {
                    language = System.Globalization.CultureInfo.CurrentCulture.ToString();
                }
            }

            ConfigHelper.SetKey(KEY_OF_LANGUAGE, language);
            _lastLanguage = language;

            var culture = new System.Globalization.CultureInfo(language);
            I18nManager.Instance.CurrentUICulture = culture;
        }
    }
}
