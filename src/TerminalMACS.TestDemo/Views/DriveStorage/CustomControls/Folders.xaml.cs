using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.DriveStorage.CustomControls
{
	/// <summary>
	/// Interaction logic for Folders.xaml
	/// </summary>
	public partial class Folders : UserControl
    {
        public Folders()
        {
            InitializeComponent();
        }


        public PathGeometry FolderIcon
        {
            get { return (PathGeometry)GetValue(FolderIconProperty); }
            set { SetValue(FolderIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderIconProperty =
            DependencyProperty.Register("FolderIcon", typeof(PathGeometry), typeof(Folders));



        public string FolderName
        {
            get { return (string)GetValue(FolderNameProperty); }
            set { SetValue(FolderNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderNameProperty =
            DependencyProperty.Register("FolderName", typeof(string), typeof(Folders));


        //Since Padding is already a property and we are using same name here
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Padding.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(Thickness), typeof(Folders));



        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Folders));



        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(Folders));


    }
}