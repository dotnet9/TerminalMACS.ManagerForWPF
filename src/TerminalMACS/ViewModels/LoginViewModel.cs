using Prism.Mvvm;
using WpfExtensions.Xaml;

namespace TerminalMACS.ViewModels
{
    class LoginViewModel : BindableBase
    {
        private string _title = "Login";
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    SetProperty(ref _title, value);
                }
            }
        }

        public LoginViewModel()
        {
            this.Title = I18nManager.Instance.Get(I18nResources.Language.AppTitle).ToString();
        }
    }
}
