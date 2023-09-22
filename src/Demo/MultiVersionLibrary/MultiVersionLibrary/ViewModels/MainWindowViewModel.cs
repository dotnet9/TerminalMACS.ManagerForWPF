using ReactiveUI;
using TestDll;
using static System.Int32;

namespace MultiVersionLibrary.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly TestTool _testTool = new();

    private string? _number;

    public string? Number
    {
        get { return _number; }
        set
        {
            _number = value;
            TryParse(_number, out var factNumber);
            Message = _testTool.GetNumberSentence(factNumber);
        }
    }

    private string? _message;

    public string? Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}