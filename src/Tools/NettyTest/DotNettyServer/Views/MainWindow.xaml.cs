using DotNettyServer.ViewModel;
using Panuon.UI.Silver;
using System.Threading;

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
            if (this.DataContext == null)
            {
                this.DataContext = ViewModel = new MainWindowViewModel();
                ViewModel.DotNettyServerHandler.RecordLogEvent += ReceiveLog;
            }
        }

        private void ReceiveLog(bool isRight, string msg)
        {
            if (this.tbLog == null || this.tbLog.Dispatcher == null)
            {
                return;
            }
            this.tbLog.Dispatcher.Invoke(() =>
            {
                string time = System.DateTime.Now.ToString("HH:mm:ss.fff");
                string formatStr = $"{time}：{msg}\r\n";
                this.tbLog.AppendText(formatStr);
                this.tbLog.ScrollToEnd();
            });
        }
    }
}
