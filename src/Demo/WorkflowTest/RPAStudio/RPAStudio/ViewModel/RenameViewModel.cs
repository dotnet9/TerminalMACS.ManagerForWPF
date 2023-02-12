using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class RenameViewModel : ViewModelBase
    {
        private ProjectViewModel _projectViewModel;
        private DocksViewModel _docksViewModel;

        /// <summary>
        /// Initializes a new instance of the RenameViewModel class.
        /// </summary>
        public RenameViewModel(ProjectViewModel projectViewModel, DocksViewModel docksViewModel)
        {
            _projectViewModel = projectViewModel;
            _docksViewModel = docksViewModel;
        }


        /// <summary>
        /// 对应的视图
        /// </summary>
        private Window _view;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 新路径
        /// </summary>
        public string NewPath { get; set; }

        /// <summary>
        /// 是否是目录
        /// </summary>
        public bool IsDirectory { get; internal set; }

        /// <summary>
        /// 目录位置
        /// </summary>
        public string Dir { get; internal set; }

        /// <summary>
        /// 是否是主文件
        /// </summary>
        public bool IsMain { get; internal set; }

        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// 窗体加载完成
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



        private RelayCommand<RoutedEventArgs> _dstNameLoadedCommand;

        /// <summary>
        /// 目标名称编辑框加载完成后触发
        /// </summary>
        public RelayCommand<RoutedEventArgs> DstNameLoadedCommand
        {
            get
            {
                return _dstNameLoadedCommand
                    ?? (_dstNameLoadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        var textBox = (TextBox)p.Source;
                        textBox.Focus();
                        textBox.SelectAll();
                    }));
            }
        }




        /// <summary>
        /// The <see cref="SrcName" /> property's name.
        /// </summary>
        public const string SrcNamePropertyName = "SrcName";

        private string _srcNameProperty = "";

        /// <summary>
        /// 源名称
        /// </summary>
        public string SrcName
        {
            get
            {
                return _srcNameProperty;
            }

            set
            {
                if (_srcNameProperty == value)
                {
                    return;
                }

                _srcNameProperty = value;
                RaisePropertyChanged(SrcNamePropertyName);
            }
        }




        /// <summary>
        /// The <see cref="IsDstNameCorrect" /> property's name.
        /// </summary>
        public const string IsDstNameCorrectPropertyName = "IsDstNameCorrect";

        private bool _isDstNameCorrectProperty = false;

        /// <summary>
        /// 目标名称是否正确 
        /// </summary>
        public bool IsDstNameCorrect
        {
            get
            {
                return _isDstNameCorrectProperty;
            }

            set
            {
                if (_isDstNameCorrectProperty == value)
                {
                    return;
                }

                _isDstNameCorrectProperty = value;
                RaisePropertyChanged(IsDstNameCorrectPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="DstNameValidatedWrongTip" /> property's name.
        /// </summary>
        public const string DstNameValidatedWrongTipPropertyName = "DstNameValidatedWrongTip";

        private string _dstNameValidatedWrongTipProperty = "";

        /// <summary>
        /// 目标名称校验错误时的提示信息
        /// </summary>
        public string DstNameValidatedWrongTip
        {
            get
            {
                return _dstNameValidatedWrongTipProperty;
            }

            set
            {
                if (_dstNameValidatedWrongTipProperty == value)
                {
                    return;
                }

                _dstNameValidatedWrongTipProperty = value;
                RaisePropertyChanged(DstNameValidatedWrongTipPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="DstName" /> property's name.
        /// </summary>
        public const string DstNamePropertyName = "DstName";

        private string _dstNameProperty = "";

        /// <summary>
        /// 目标名称
        /// </summary>
        public string DstName
        {
            get
            {
                return _dstNameProperty;
            }

            set
            {
                if (_dstNameProperty == value)
                {
                    return;
                }

                _dstNameProperty = value;
                RaisePropertyChanged(DstNamePropertyName);

                dstNameValidate(value);
            }
        }

        /// <summary>
        /// 目录名称校验
        /// </summary>
        /// <param name="value">值</param>
        private void dstNameValidate(string value)
        {
            IsDstNameCorrect = true;

            if (string.IsNullOrEmpty(value))
            {
                IsDstNameCorrect = false;
                DstNameValidatedWrongTip = "名称不能为空";
            }
            else
            {
                if (value.Contains(@"\") || value.Contains(@"/"))
                {
                    IsDstNameCorrect = false;
                    DstNameValidatedWrongTip = "名称不能有非法字符";
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
                        IsDstNameCorrect = false;
                        DstNameValidatedWrongTip = "名称不能有非法字符";
                    }
                }
            }

            var dstFullPath = Dir + @"\" + DstName;
            if (Directory.Exists(dstFullPath))
            {
                IsDstNameCorrect = false;
                DstNameValidatedWrongTip = "相同名字的目录已存在";
            }
            else if (File.Exists(dstFullPath))
            {
                IsDstNameCorrect = false;
                DstNameValidatedWrongTip = "相同名字的文件已存在";
            }
        }


        private RelayCommand _okCommand;

        /// <summary>
        /// 确定
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand
                    ?? (_okCommand = new RelayCommand(
                    () =>
                    {
                        if (DstName != SrcName)
                        {
                            if (IsDirectory)
                            {
                                NewPath = Dir + @"\" + DstName;
                                Directory.Move(Dir + @"\" + SrcName, NewPath);
                            }
                            else
                            {
                                NewPath = Dir + @"\" + DstName;
                                File.Move(Dir + @"\" + SrcName, NewPath);
                            }

                            _projectViewModel.OnRename(this);
                            _docksViewModel.OnRename(this);
                        }

                        _view.Close();
                    },
                    () => IsDstNameCorrect));
            }
        }



        private RelayCommand _cancelCommand;

        /// <summary>
        /// 取消
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
                    }));
            }
        }


    }
}