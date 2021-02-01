using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TerminalMACS.TestDemo.Views.DriveStorage.CustomControls
{
	/// <summary>
	/// Interaction logic for MediumCard.xaml
	/// </summary>
	public partial class MediumCard : UserControl
    {
        public MediumCard()
        {
            InitializeComponent();
        }


        public PathGeometry FileIcon
        {
            get { return (PathGeometry)GetValue(FileIconProperty); }
            set { SetValue(FileIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileIconProperty =
            DependencyProperty.Register("FileIcon", typeof(PathGeometry), typeof(MediumCard));



        public SolidColorBrush Fill
        {
            get { return (SolidColorBrush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(SolidColorBrush), typeof(MediumCard));



        public string Text1
        {
            get { return (string)GetValue(Text1Property); }
            set { SetValue(Text1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Text1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Text1Property =
            DependencyProperty.Register("Text1", typeof(string), typeof(MediumCard));

        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Text1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(MediumCard));
    }
}
