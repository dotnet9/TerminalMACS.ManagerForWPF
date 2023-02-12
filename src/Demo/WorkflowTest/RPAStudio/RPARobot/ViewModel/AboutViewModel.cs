using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Shared.Utils;
using System.ComponentModel;
using System.Windows;

namespace RPARobot.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        /// <summary>
        /// 对应的视图
        /// </summary>
        public Window m_view { get; set; }

        /// <summary>
        /// Initializes a new instance of the AboutViewModel class.
        /// </summary>
        public AboutViewModel()
        {
            ProgramVersion = Common.GetProgramVersion();
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
                    }));
            }
        }

        private RelayCommand _MouseLeftButtonDownCommand;

        /// <summary>
        /// 鼠标左键按下处理
        /// </summary>
        public RelayCommand MouseLeftButtonDownCommand
        {
            get
            {
                return _MouseLeftButtonDownCommand
                    ?? (_MouseLeftButtonDownCommand = new RelayCommand(
                    () =>
                    {
                        //点标题外的部分也能拖动，方便使用
                        m_view.DragMove();
                    }));
            }
        }



        private RelayCommand<CancelEventArgs> _closingCommand;

        /// <summary>
        /// 窗体即将关闭时触发
        /// </summary>
        public RelayCommand<CancelEventArgs> ClosingCommand
        {
            get
            {
                return _closingCommand
                    ?? (_closingCommand = new RelayCommand<CancelEventArgs>(
                    e =>
                    {
                        e.Cancel = true;//不关闭窗口
                        m_view.Hide();
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

    }

}