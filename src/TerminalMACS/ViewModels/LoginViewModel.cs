using WPFXmlTranslator;

namespace TerminalMACS.ViewModels;

internal class LoginViewModel : BindableBase
{
    private string? _title = "Login";

    public LoginViewModel()
    {
        Title = I18nManager.Instance.GetResource(Localization.MainWindow.Title);
    }

    public string? Title
    {
        get => _title;
        set
        {
            if (value != _title) SetProperty(ref _title, value);
        }
    }
}