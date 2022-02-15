using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace TerminalMACS.TestDemo.ViewModels.TestBehaviors;

public class TestBehaviorsViewModel : BindableBase
{
    private string _CurrentText;

    public TestBehaviorsViewModel()
    {
        ButtonClickCommand = new DelegateCommand(() => MessageBox.Show("DD"));
    }

    public string CurrentText
    {
        get => _CurrentText;
        set => SetProperty(ref _CurrentText, value);
    }


    public ICommand ButtonClickCommand { get; }
}