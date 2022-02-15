using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.DriveStorage.CustomControls;

/// <summary>
///     Interaction logic for Folders.xaml
/// </summary>
public partial class Folders : UserControl
{
    // Using a DependencyProperty as the backing store for FolderIcon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FolderIconProperty =
        DependencyProperty.Register("FolderIcon", typeof(PathGeometry), typeof(Folders));

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FolderNameProperty =
        DependencyProperty.Register("FolderName", typeof(string), typeof(Folders));

    // Using a DependencyProperty as the backing store for Padding.  This enables animation, styling, binding, etc...
    public new static readonly DependencyProperty PaddingProperty =
        DependencyProperty.Register("Padding", typeof(Thickness), typeof(Folders));

    // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register("IsSelected", typeof(bool), typeof(Folders));

    // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register("GroupName", typeof(string), typeof(Folders));

    public Folders()
    {
        InitializeComponent();
    }


    public PathGeometry FolderIcon
    {
        get => (PathGeometry)GetValue(FolderIconProperty);
        set => SetValue(FolderIconProperty, value);
    }


    public string FolderName
    {
        get => (string)GetValue(FolderNameProperty);
        set => SetValue(FolderNameProperty, value);
    }


    //Since Padding is already a property and we are using same name here
    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }


    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }


    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }
}