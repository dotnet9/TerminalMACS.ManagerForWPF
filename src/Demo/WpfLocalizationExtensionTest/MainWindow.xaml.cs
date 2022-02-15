using System.Globalization;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace WpfLocalizationExtensionTest;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool isEn = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonGet_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(LangHelper.GetLocalizedString("Test"));
    }

    private void ChangeLanguge_Click(object sender, RoutedEventArgs e)
    {
        var lan = isEn ? "en-US" : "zh-CN";
        isEn = !isEn;
        LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
        LocalizeDictionary.Instance.Culture =
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(lan);
    }
}