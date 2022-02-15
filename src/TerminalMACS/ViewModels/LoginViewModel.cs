using Prism.Mvvm;
using TerminalMACS.I18nResources;
using WpfExtensions.Xaml;

namespace TerminalMACS.ViewModels;

internal class LoginViewModel : BindableBase
{
    private string _title = "Login";

    public LoginViewModel()
    {
        Title = I18nManager.Instance.Get(Language.AppTitle).ToString();
    }

    public string Title
    {
        get => _title;
        set
        {
            if (value != _title) SetProperty(ref _title, value);
        }
    }
}