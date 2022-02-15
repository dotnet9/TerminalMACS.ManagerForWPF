using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace TerminalMACS.TestDemo.Views.Tree;

/// <summary>
///     menu type
/// </summary>
public enum CusMenuType
{
    RemoveChild = 1
}

/// <summary>
///     the class of client brower view menu details info
/// </summary>
public class CusMenuInfo : BindableBase
{
    private ObservableCollection<CusMenuInfo> _Children;
    private bool _IsEnabled;

    private bool _IsSelected;

    private string _Name;

    public CusMenuInfo(int level, string name, string icon)
    {
        Level = level;
        Name = name;
        Icon = icon;
    }

    public int Level { get; set; }

    /// <summary>
    ///     get or set the client type
    /// </summary>
    public string ClientType { get; set; }

    /// <summary>
    ///     get or set the menu type
    /// </summary>
    public int MenuType { get; set; }

    /// <summary>
    ///     获取或者设置菜单是否可用
    /// </summary>
    public bool IsEnabled
    {
        get => _IsEnabled;
        set => SetProperty(ref _IsEnabled, value);
    }

    /// <summary>
    ///     get or set the menu name
    /// </summary>
    public string Name
    {
        get => _Name;
        set => SetProperty(ref _Name, value);
    }

    /// <summary>
    ///     get or set the children
    /// </summary>
    public ObservableCollection<CusMenuInfo> Children
    {
        get => _Children;
        set => SetProperty(ref _Children, value);
    }

    /// <summary>
    ///     new IsSelected
    /// </summary>
    public bool IsSelected
    {
        get => _IsSelected;
        set => SetProperty(ref _IsSelected, value);
    }

    public string Icon { get; set; }

    /// <summary>
    ///     获取或者父级目录
    /// </summary>
    public CusMenuInfo Parent { get; set; }
}