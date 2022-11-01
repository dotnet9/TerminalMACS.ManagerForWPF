using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace WpfAppForZoomInAndZoomOut.ViewModels;

public class TypeTreeModel : TypeModel
{
    public ObservableCollection<TypeTreeModel> ChildList { get; set; } = new();
}

public class TypeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
}

public class ComboBoxWithTreeViewDemoViewModel : BindableBase
{
    private int comboSelected;

    private TypeModel selectItem;

    private string showName;

    public ComboBoxWithTreeViewDemoViewModel()
    {
        TypeList = new ObservableCollection<TypeTreeModel>(GetData());

        SelectItemChangeCommand = new DelegateCommand<TypeModel>(onSelectItemChange);
        SelectionChangedCommand = new DelegateCommand(onSelectionChanged);
    }

    public ObservableCollection<TypeTreeModel> TypeList { get; set; } = new();

    public DelegateCommand<TypeModel> SelectItemChangeCommand { get; set; }
    public ICommand? SelectionChangedCommand { get; set; }

    public TypeModel SelectItem
    {
        get => selectItem;
        set => SetProperty(ref selectItem, value);
    }

    public string ShowName
    {
        get => showName;
        set => SetProperty(ref showName, value);
    }

    public int ComboSelected
    {
        get => comboSelected;
        set => SetProperty(ref comboSelected, value);
    }


    private List<TypeTreeModel> GetData()
    {
        List<TypeTreeModel> typeTrees = new()
        {
            new()
            {
                Id = 1,
                Name = "手机",
                ChildList = new ObservableCollection<TypeTreeModel>
                {
                    new() { Id = 2, Name = "苹果" },
                    new()
                    {
                        Id = 3, Name = "华为",
                        ChildList = new ObservableCollection<TypeTreeModel>
                        {
                            new() { Id = 4, Name = "荣耀" }
                        }
                    },
                    new()
                    {
                        Id = 5, Name = "小米",
                        ChildList = new ObservableCollection<TypeTreeModel>
                        {
                            new() { Id = 6, Name = "红米" }
                        }
                    }
                }
            },
            new()
            {
                Id = 7,
                Name = "笔记本",
                ChildList = new ObservableCollection<TypeTreeModel>
                {
                    new() { Id = 8, Name = "联想" }
                }
            },
            new()
            {
                Id = 9,
                Name = "耳机"
            }
        };
        return typeTrees;
    }

    private void onSelectItemChange(TypeModel type)
    {
        SelectItem = type;
        ShowName = SelectItem.Name;
    }

    private void onSelectionChanged()
    {
        ComboSelected = 0;
    }
}