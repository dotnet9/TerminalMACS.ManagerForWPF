using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class NewFolderViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private Window _view;

        public string Path { get; internal set; }

        private ProjectViewModel _projectViewModel;


        /// <summary>
        /// Initializes a new instance of the NewFolderViewModel class.
        /// </summary>
        public NewFolderViewModel(ProjectViewModel projectViewModel)
        {
            _projectViewModel = projectViewModel;
        }


        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// 窗体加载事件完成后触发
        /// </summary>
        public RelayCommand<RoutedEventArgs> LoadedCommand
        {
            get
            {
                return _loadedCommand
                    ?? (_loadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        _view = (Window)p.Source;
                    }));
            }
        }


        private RelayCommand<RoutedEventArgs> _folderNameLoadedCommand;

        /// <summary>
        /// TextBox加载完成后调用
        /// </summary>
        public RelayCommand<RoutedEventArgs> FolderNameLoadedCommand
        {
            get
            {
                return _folderNameLoadedCommand
                    ?? (_folderNameLoadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        var textBox = (TextBox)p.Source;
                        textBox.Focus();
                        textBox.SelectAll();
                    }));
            }
        }



        /// <summary>
        /// The <see cref="IsFolderNameCorrect" /> property's name.
        /// </summary>
        public const string IsFolderNameCorrectPropertyName = "IsFolderNameCorrect";

        private bool _isFolderNameCorrectProperty = false;

        /// <summary>
        /// 目录名称是否正确
        /// </summary>
        public bool IsFolderNameCorrect
        {
            get
            {
                return _isFolderNameCorrectProperty;
            }

            set
            {
                if (_isFolderNameCorrectProperty == value)
                {
                    return;
                }

                _isFolderNameCorrectProperty = value;
                RaisePropertyChanged(IsFolderNameCorrectPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="FolderNameValidatedWrongTip" /> property's name.
        /// </summary>
        public const string FolderNameValidatedWrongTipPropertyName = "FolderNameValidatedWrongTip";

        private string _folderNameValidatedWrongTipProperty = "";

        /// <summary>
        /// 目录名称非法时的提示信息
        /// </summary>
        public string FolderNameValidatedWrongTip
        {
            get
            {
                return _folderNameValidatedWrongTipProperty;
            }

            set
            {
                if (_folderNameValidatedWrongTipProperty == value)
                {
                    return;
                }

                _folderNameValidatedWrongTipProperty = value;
                RaisePropertyChanged(FolderNameValidatedWrongTipPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="FolderName" /> property's name.
        /// </summary>
        public const string FolderNamePropertyName = "FolderName";

        private string _folderNameProperty = "";

        /// <summary>
        /// 目录名称
        /// </summary>
        public string FolderName
        {
            get
            {
                return _folderNameProperty;
            }

            set
            {
                if (_folderNameProperty == value)
                {
                    return;
                }

                _folderNameProperty = value;
                RaisePropertyChanged(FolderNamePropertyName);

                folderNameValidate(value);
            }
        }

        /// <summary>
        /// 目录名称有效性检查
        /// </summary>
        /// <param name="value">待检查的名称</param>
        private void folderNameValidate(string value)
        {
            IsFolderNameCorrect = true;

            if (string.IsNullOrEmpty(value))
            {
                IsFolderNameCorrect = false;
                FolderNameValidatedWrongTip = "名称不能为空";
            }
            else
            {
                if (value.Contains(@"\") || value.Contains(@"/"))
                {
                    IsFolderNameCorrect = false;
                    FolderNameValidatedWrongTip = "名称不能有非法字符";
                }
                else
                {
                    System.IO.FileInfo fi = null;
                    try
                    {
                        fi = new System.IO.FileInfo(value);
                    }
                    catch (ArgumentException) { }
                    catch (System.IO.PathTooLongException) { }
                    catch (NotSupportedException) { }
                    if (ReferenceEquals(fi, null))
                    {
                        IsFolderNameCorrect = false;
                        FolderNameValidatedWrongTip = "名称不能有非法字符";
                    }
                }
            }

            if (Directory.Exists(Path + @"\" + FolderName))
            {
                IsFolderNameCorrect = false;
                FolderNameValidatedWrongTip = "已经存在同名称的目录";
            }
        }

        private RelayCommand _okCommand;

        /// <summary>
        /// 确定命令
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand
                    ?? (_okCommand = new RelayCommand(
                    () =>
                    {
                        //新建文件夹，然后刷新项目文件显示
                        Directory.CreateDirectory(Path + @"\" + FolderName);
                        _projectViewModel.Refresh();

                        _view.Close();
                    },
                    () => IsFolderNameCorrect));
            }
        }



        private RelayCommand _cancelCommand;

        /// <summary>
        /// 取消命令
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                    ?? (_cancelCommand = new RelayCommand(
                    () =>
                    {
                        _view.Close();
                    },
                    () => true));
            }
        }
    }


}