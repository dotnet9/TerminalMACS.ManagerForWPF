using DotNettyClient.ViewModel;
using Panuon.UI.Silver;

namespace DotNettyClient.Views
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
            if (this.DataContext == null)
            {
                this.DataContext = ViewModel = new MainWindowViewModel();
                ViewModel.DotNettyClientHandler.RecordLogEvent += ReceiveLog;
            }
        }

        private void ReceiveLog(string msg)
        {
            this.tbLog.Dispatcher.Invoke(() =>
            {
                string time = System.DateTime.Now.ToString("HH:mm:ss.fff");
                string formatStr = $"{time}：{msg}\r\n";
                this.tbLog.AppendText(formatStr);
            });
        }
    }
}
