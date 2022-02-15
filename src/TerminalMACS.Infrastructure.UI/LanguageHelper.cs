using System.Globalization;
using WpfExtensions.Xaml;

namespace TerminalMACS.Infrastructure.UI;

public class LanguageHelper
{
    private const string KEY_OF_LANGUAGE = "language";
    private static string _lastLanguage = "";

    public static void SetLanguage(string language = "")
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            language = ConfigHelper.ReadKey(KEY_OF_LANGUAGE);
            if (string.IsNullOrWhiteSpace(language)) language = CultureInfo.CurrentCulture.ToString();
        }

        ConfigHelper.SetKey(KEY_OF_LANGUAGE, language);
        _lastLanguage = language;

        var culture = new CultureInfo(language);
        I18nManager.Instance.CurrentUICulture = culture;
    }
}