using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class TestWindowsViewModel : BindableBase
{
    private readonly List<AddressViewModel> _addressViewModels;
    private ObservableCollection<string>? _address;

    private string? _selectedAddress;

    private AddressViewModel? _selectedAddressViewModel;
    private ICommand? showMessageCommand;

    public TestWindowsViewModel()
    {
        Address = new ObservableCollection<string>(new[] { "张三", "李四", "王五" });
        _addressViewModels = new List<AddressViewModel>();
        for (var i = 0; i < 3; i++) _addressViewModels.Add(new AddressViewModel { Name = Address[i] });
        SelectedAddress = Address[0];
    }

    public ObservableCollection<string>? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string? SelectedAddress
    {
        get => _selectedAddress;
        set
        {
            SetProperty(ref _selectedAddress, value);
            SelectedAddressViewModel = _addressViewModels[_address!.IndexOf(value!)];
        }
    }

    public AddressViewModel? SelectedAddressViewModel
    {
        get => _selectedAddressViewModel;
        set => SetProperty(ref _selectedAddressViewModel, value);
    }

    public ICommand ShowMessageCommand =>
        showMessageCommand ??= new DelegateCommand<object>(ExecuteShowMessageCommand);

    private void ExecuteShowMessageCommand(object? obj)
    {
        MessageBox.Show(obj == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") : obj.ToString());
    }
}