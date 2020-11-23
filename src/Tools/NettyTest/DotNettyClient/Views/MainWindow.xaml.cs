using DotNetty.Common.Concurrency;
using DotNettyClient.DotNetty;
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
                ClientEventHandler.RecordLogEvent += ReceiveLog;
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
