using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class TestComboBoxWithDataGridViewModel : BindableBase
{
    public ObservableCollection<City> Cities { get; } = new()
    {
        new City() { Name = "Mumbai", State = "Maharashtra", Population = 3000000 },
        new City() { Name = "Pune", State = "Maharashtra", Population = 7000000 },
        new City() { Name = "Nashik", State = "Maharashtra", Population = 65000 },
        new City() { Name = "Aurangabad", State = "Maharashtra", Population = 5000000 }
    };
}

public class City
{
    public string State { get; set; }
    public string Name { get; set; }
    public int Population { get; set; }
}