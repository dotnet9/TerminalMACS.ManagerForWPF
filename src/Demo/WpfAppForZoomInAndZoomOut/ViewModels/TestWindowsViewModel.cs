using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class TestWindowsViewModel : BindableBase
{
    private ObservableCollection<string>? _address;

    private readonly List<AddressViewModel> _addressViewModels;

    private string? _selectedAddress;

    private AddressViewModel? _selectedAddressViewModel;

    public TestWindowsViewModel()
    {
        Address = new ObservableCollection<string>(new[] { "张三", "李四", "王五" });
        _addressViewModels = new List<AddressViewModel>();
        for (var i = 0; i < 3; i++) _addressViewModels.Add(new AddressViewModel { Name = Address[i] });
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
}