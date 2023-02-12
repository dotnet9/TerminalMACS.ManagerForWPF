using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Shared.Configs;
using RPA.Shared.Extensions;
using RPA.Shared.Utils;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class NewXamlFileViewModel : ViewModelBase
    {
        private ProjectViewModel _projectViewModel;
        private readonly Regex _disallowedCharacters = new Regex("[\\W-]");
        /// <summary>
        /// Initializes a new instance of the NewXamlFileViewModel class.
        /// </summary>
        public NewXamlFileViewModel(ProjectViewModel projectViewModel)
        {
            _projectViewModel = projectViewModel;
        }


        /// <summary>
        /// 对应的视图
        /// </summary>
        private Window _view;

        /// <summary>
        /// 项目路径
        /// </summary>
        public string ProjectPath { get; internal set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public enum enFileType
        {
            Null = 0,
            Sequence,//序列
            Flowchart,//流程图
            StateMachine,//状态机
        }



        private RelayCommand<RoutedEventArgs> _loadedCommand;

        /// <summary>
        /// 窗体加载完成后触发
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


        private RelayCommand<RoutedEventArgs> _fileNameLoadedCommand;

        /// <summary>
        /// textBox加载完成后触发
        /// </summary>
        public RelayCommand<RoutedEventArgs> FileNameLoadedCommand
        {
            get
            {
                return _fileNameLoadedCommand
                    ?? (_fileNameLoadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        var textBox = (TextBox)p.Source;
                        textBox.Focus();
                        textBox.SelectAll();
                    }));
            }
        }



        /// <summary>
        /// The <see cref="FileType" /> property's name.
        /// </summary>
        public const string FileTypePropertyName = "FileType";

        private enFileType _fileTypeProperty = enFileType.Null;

        /// <summary>
        /// 文件类型
        /// </summary>
        public enFileType FileType
        {
            get
            {
                return _fileTypeProperty;
            }

            set
            {
                if (_fileTypeProperty == value)
                {
                    return;
                }

                _fileTypeProperty = value;
                RaisePropertyChanged(FileTypePropertyName);

                initInfoByFileType(value);
            }
        }

        /// <summary>
        /// 通过文件类型来初始化信息
        /// </summary>
        /// <param name="value"></param>
        private void initInfoByFileType(enFileType value)
        {
            var nodeTypeStr = value.ToString();
            
            Title = SystemKeyValueConfig.GetValue($"{nodeTypeStr}.Title");
            Description = SystemKeyValueConfig.GetValue($"{nodeTypeStr}.Description");
            var fileName = SystemKeyValueConfig.GetValue($"{nodeTypeStr}.FileName");

            var newfileNameWithExt = Common.GetValidFileName(FilePath, fileName + ProjectConstantConfig.XamlFileExtension, "", "{0}", 1);
            FileName = Path.GetFileNameWithoutExtension(newfileNameWithExt);
        }



        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _titleProperty = "";

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return _titleProperty;
            }

            set
            {
                if (_titleProperty == value)
                {
                    return;
                }

                _titleProperty = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }



        /// <summary>
        /// The <see cref="Description" /> property's name.
        /// </summary>
        public const string DescriptionPropertyName = "Description";

        private string _descriptionProperty = "";

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return _descriptionProperty;
            }

            set
            {
                if (_descriptionProperty == value)
                {
                    return;
                }

                _descriptionProperty = value;
                RaisePropertyChanged(DescriptionPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="IsFileNameCorrect" /> property's name.
        /// </summary>
        public const string IsFileNameCorrectPropertyName = "IsFileNameCorrect";

        private bool _isFileNameCorrectProperty = false;

        /// <summary>
        /// 文件名称是否正确
        /// </summary>
        public bool IsFileNameCorrect
        {
            get
            {
                return _isFileNameCorrectProperty;
            }

            set
            {
                if (_isFileNameCorrectProperty == value)
                {
                    return;
                }

                _isFileNameCorrectProperty = value;
                RaisePropertyChanged(IsFileNameCorrectPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="FileNameValidatedWrongTip" /> property's name.
        /// </summary>
        public const string FileNameValidatedWrongTipPropertyName = "FileNameValidatedWrongTip";

        private string _fileNameValidatedWrongTipProperty = "";

        /// <summary>
        /// 文件名称校验错误时的提示
        /// </summary>
        public string FileNameValidatedWrongTip
        {
            get
            {
                return _fileNameValidatedWrongTipProperty;
            }

            set
            {
                if (_fileNameValidatedWrongTipProperty == value)
                {
                    return;
                }

                _fileNameValidatedWrongTipProperty = value;
                RaisePropertyChanged(FileNameValidatedWrongTipPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="FileName" /> property's name.
        /// </summary>
        public const string FileNamePropertyName = "FileName";

        private string _fileNameProperty = "";

        /// <summary>
        /// 文件名称 
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileNameProperty;
            }

            set
            {
                if (_fileNameProperty == value)
                {
                    return;
                }

                _fileNameProperty = value;
                RaisePropertyChanged(FileNamePropertyName);

                fileNameValidate(value);
            }
        }

        /// <summary>
        /// 文件名称校验
        /// </summary>
        /// <param name="value">待检验的值</param>
        private void fileNameValidate(string value)
        {
            IsFileNameCorrect = true;

            if (string.IsNullOrEmpty(value))
            {
                IsFileNameCorrect = false;
                FileNameValidatedWrongTip = "名称不能为空";
            }
            else
            {
                if (value.Contains(@"\") || value.Contains(@"/"))
                {
                    IsFileNameCorrect = false;
                    FileNameValidatedWrongTip = "名称不能有非法字符";
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
                        IsFileNameCorrect = false;
                        FileNameValidatedWrongTip = "名称不能有非法字符";
                    }
                }
            }

            if (File.Exists(FilePath + @"\" + FileName + ProjectConstantConfig.XamlFileExtension))
            {
                IsFileNameCorrect = false;
                FileNameValidatedWrongTip = "指定的文件已存在";
            }
        }




        /// <summary>
        /// The <see cref="IsFilePathCorrect" /> property's name.
        /// </summary>
        public const string IsFilePathCorrectPropertyName = "IsFilePathCorrect";

        private bool _isFilePathCorrectProperty = false;

        /// <summary>
        /// 文件路径是否正确
        /// </summary>
        public bool IsFilePathCorrect
        {
            get
            {
                return _isFilePathCorrectProperty;
            }

            set
            {
                if (_isFilePathCorrectProperty == value)
                {
                    return;
                }

                _isFilePathCorrectProperty = value;
                RaisePropertyChanged(IsFilePathCorrectPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="FilePathValidatedWrongTip" /> property's name.
        /// </summary>
        public const string FilePathValidatedWrongTipPropertyName = "FilePathValidatedWrongTip";

        private string _filePathValidatedWrongTipProperty = "";

        /// <summary>
        /// 文件路径校验错误时的提示
        /// </summary>
        public string FilePathValidatedWrongTip
        {
            get
            {
                return _filePathValidatedWrongTipProperty;
            }

            set
            {
                if (_filePathValidatedWrongTipProperty == value)
                {
                    return;
                }

                _filePathValidatedWrongTipProperty = value;
                RaisePropertyChanged(FilePathValidatedWrongTipPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="FilePath" /> property's name.
        /// </summary>
        public const string FilePathPropertyName = "FilePath";

        private string _filePathProperty = "";

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePathProperty;
            }

            set
            {
                if (_filePathProperty == value)
                {
                    return;
                }

                _filePathProperty = value;
                RaisePropertyChanged(FilePathPropertyName);

                filePathValidate(value);
                fileNameValidate(FileName);
            }
        }

        /// <summary>
        /// 文件路径合法性校验
        /// </summary>
        /// <param name="value">待校验的值</param>
        private void filePathValidate(string value)
        {
            IsFilePathCorrect = true;
            if (string.IsNullOrEmpty(value))
            {
                IsFilePathCorrect = false;
                FilePathValidatedWrongTip = "位置不能为空";
            }
            else
            {
                if (!Directory.Exists(value))
                {
                    IsFilePathCorrect = false;
                    FilePathValidatedWrongTip = "指定的位置不存在";
                }
            }

            //判断是否是在项目目录中的子目录里
            if (!value.IsSubPathOf(ProjectPath))
            {
                IsFilePathCorrect = false;
                FilePathValidatedWrongTip = "指定的位置必须是项目所在目录或其子目录中";
            }
        }

        private RelayCommand _selectFilePathCommand;

        /// <summary>
        /// 选择文件路径命令
        /// </summary>
        public RelayCommand SelectFilePathCommand
        {
            get
            {
                return _selectFilePathCommand
                    ?? (_selectFilePathCommand = new RelayCommand(
                    () =>
                    {
                        string dst_dir = "";
                        if (CommonDialog.ShowSelectDirDialog(Title, ref dst_dir))
                        {
                            FilePath = dst_dir;
                        }
                    }));
            }
        }



        private RelayCommand _createFileCommand;

        /// <summary>
        /// 创建文件命令
        /// </summary>
        public RelayCommand CreateFileCommand
        {
            get
            {
                return _createFileCommand
                    ?? (_createFileCommand = new RelayCommand(
                    () =>
                    {
                        var xamlFilePath = newXamlFile();

                        _projectViewModel.Refresh();

                        if (!string.IsNullOrEmpty(xamlFilePath))
                        {
                            var item = _projectViewModel.GetProjectItemByFullPath(xamlFilePath);
                            if (item is ProjectFileItemViewModel)
                            {
                                (item as ProjectFileItemViewModel).OpenXamlCommand.Execute(null);
                            }
                        }

                        _view.Close();
                    },
                    () => IsFileNameCorrect && IsFilePathCorrect));
            }
        }






        /// <summary>
        /// 新建XAML文件
        /// </summary>
        /// <returns>创建后的文件路径</returns>
        private string newXamlFile()
        {
            var retPath = "";
            var content = "";

            var fileName = FileName.Trim();
            var title = _disallowedCharacters.Replace(fileName, "_");

            switch (FileType)
            {
                case enFileType.Sequence:
                    content = Common.GetResourceContentByUri($"pack://application:,,,/RPA.Resources;Component/FileTemplates/Sequence.xaml");
                    break;
                case enFileType.Flowchart:
                    content = Common.GetResourceContentByUri($"pack://application:,,,/RPA.Resources;Component/FileTemplates/Flowchart.xaml");
                    break;
                case enFileType.StateMachine:
                    content = Common.GetResourceContentByUri($"pack://application:,,,/RPA.Resources;Component/FileTemplates/StateMachine.xaml");
                    break;
            }

            if(!string.IsNullOrEmpty(content))
            {
                if (char.IsDigit(title[0]))
                {
                    content = content.Replace("{{title}}", "_" + title);
                }
                else
                {
                    content = content.Replace("{{title}}", title);
                }

                retPath = FilePath + @"\" + fileName + ProjectConstantConfig.XamlFileExtension;
                File.WriteAllText(retPath,content);
            }

            return retPath;

        }

    }
}