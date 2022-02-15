using Prism.Mvvm;

namespace TerminalMACS.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private string _title = "test";

    public string Title
    {
        get => _title;
        set
        {
            if (value != _title) SetProperty(ref _title, value);
        }
    }
}