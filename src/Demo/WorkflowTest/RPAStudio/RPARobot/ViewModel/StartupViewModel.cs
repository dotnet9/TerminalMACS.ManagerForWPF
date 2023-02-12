using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using RPA.Interfaces.Service;
using RPA.Shared.Utils;
using RPARobot.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPARobot.ViewModel
{
    public class StartupViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;

        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 对应的视图
        /// </summary>
        public Window m_view { get; set; }

        /// <summary>
        /// 主窗体
        /// </summary>
        public MainWindow MainWindow { get; set; }

        /// <summary>
        /// 关于窗体
        /// </summary>
        public AboutWindow AboutWindow { get; set; }

        private MainViewModel _mainViewModel;

        /// <summary>
        /// Initializes a new instance of the StartupViewModel class.
        /// </summary>
        public StartupViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _mainViewModel = _serviceLocator.ResolveType<MainViewModel>();

            //初始化需要的各种窗体，这些窗体后面不会真实关闭
            if (MainWindow == null)
            {
                MainWindow = new MainWindow();
                MainWindow.Hide();
            }

            if (AboutWindow == null)
            {
                AboutWindow = new AboutWindow();
                AboutWindow.Hide();
            }

            App.Current.MainWindow = MainWindow;
        }

        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// 窗体加载完成后调用
        /// </summary>
        public RelayCommand<RoutedEventArgs> LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        m_view = (Window)p.Source;

                        Init();
                    }));
            }
        }

        

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            ProgramVersion = string.Format("RPARobot-{0}", Common.GetProgramVersion());

            ShowMainWindowCommand.Execute(null);
        }




        private RelayCommand _showMainWindowCommand;

        /// <summary>
        /// 显示主窗体
        /// </summary>
        public RelayCommand ShowMainWindowCommand
        {
            get
            {
                return _showMainWindowCommand
                    ?? (_showMainWindowCommand = new RelayCommand(
                    () =>
                    {
                        MainWindow.Show();
                        MainWindow.Activate();
                    }));
            }
        }

        private RelayCommand _quitMainWindowCommand;

        /// <summary>
        /// 退出主程序
        /// </summary>
        public RelayCommand QuitMainWindowCommand
        {
            get
            {
                return _quitMainWindowCommand
                    ?? (_quitMainWindowCommand = new RelayCommand(
                    () =>
                    {
                        var ret = CommonMessageBox.ShowWarningYesNo("确定退出吗？");
                        if (ret)
                        {
                            Application.Current.Shutdown();
                        }
                    }));
            }
        }



        private RelayCommand _viewLogsCommand;

        /// <summary>
        /// 打开日志
        /// </summary>
        public RelayCommand ViewLogsCommand
        {
            get
            {
                return _viewLogsCommand
                    ?? (_viewLogsCommand = new RelayCommand(
                    () =>
                    {
                        _mainViewModel.ViewLogsCommand.Execute(null);
                    }));
            }
        }


        private RelayCommand _aboutProductCommand;

        /// <summary>
        /// 关于产品
        /// </summary>
        public RelayCommand AboutProductCommand
        {
            get
            {
                return _aboutProductCommand
                    ?? (_aboutProductCommand = new RelayCommand(
                    () =>
                    {
                        _mainViewModel.AboutProductCommand.Execute(null);
                    }));
            }
        }



        /// <summary>
        /// The <see cref="ProgramVersion" /> property's name.
        /// </summary>
        public const string ProgramVersionPropertyName = "ProgramVersion";

        private string _programVersionProperty = "";

        /// <summary>
        /// 程序版本
        /// </summary>
        public string ProgramVersion
        {
            get
            {
                return _programVersionProperty;
            }

            set
            {
                if (_programVersionProperty == value)
                {
                    return;
                }

                _programVersionProperty = value;
                RaisePropertyChanged(ProgramVersionPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="ProgramStatus" /> property's name.
        /// </summary>
        public const string ProgramStatusPropertyName = "ProgramStatus";

        private string _programStatusProperty = "";

        /// <summary>
        /// 程序状态
        /// </summary>
        public string ProgramStatus
        {
            get
            {
                return _programStatusProperty;
            }

            set
            {
                if (_programStatusProperty == value)
                {
                    return;
                }

                _programStatusProperty = value;
                RaisePropertyChanged(ProgramStatusPropertyName);
            }
        }








    }
}