using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class AddressViewModel : BindableBase
{
    private string? _name;

    private ICommand? changeNameCommand;

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public ICommand? ChangeNameCommand => changeNameCommand ??= new DelegateCommand(ExecuteChangeNameCommand);

    private void ExecuteChangeNameCommand()
    {
        Name = DateTime.Now.ToString("HH:mm:ss fff");
    }
}