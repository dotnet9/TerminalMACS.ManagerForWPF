using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class TestUserControlCommandWindowViewModel : BindableBase
{
    private ICommand? _showMessageCommand;

    public ICommand ShowMessageCommand =>
        _showMessageCommand ??= new DelegateCommand<object>(ExecuteShowMessageCommand);

    private void ExecuteShowMessageCommand(object? obj)
    {
        MessageBox.Show(obj == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") : obj.ToString());
    }
}