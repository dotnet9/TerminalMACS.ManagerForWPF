using ReactiveUI;
using TestDll;

namespace MultiVersionLibrary.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly TestTool _testTool = new();

    private int _number = DateTime.Now.Microsecond;

    public int Number
    {
        get { return _number; }
        set
        {
            _number = value;
            Message = _testTool.TellMeOddEven(_number);
        }
    }

    private string? _message;

    public string? Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}