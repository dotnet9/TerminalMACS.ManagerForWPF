using DotNettyServer.ViewModel;
using Panuon.UI.Silver;

namespace DotNettyServer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {
        private MainWindowViewModel _ViewModel;
        public MainWindowViewModel ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            if(this.DataContext==null)
            {
                this.DataContext = ViewModel = new MainWindowViewModel();
            }
        }
    }
}
