using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.DriveStorage.CustomControls
{
	/// <summary>
	/// Interaction logic for CustomList.xaml
	/// </summary>
	public partial class CustomList : UserControl
    {
        public CustomList()
        {
            InitializeComponent();
            DataContext = new MyFilesViewModel();
        }



        public PathGeometry FileThumb
        {
            get { return (PathGeometry)GetValue(FileThumbProperty); }
            set { SetValue(FileThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileThumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileThumbProperty =
            DependencyProperty.Register("FileThumb", typeof(PathGeometry), typeof(CustomList));


    }
}
