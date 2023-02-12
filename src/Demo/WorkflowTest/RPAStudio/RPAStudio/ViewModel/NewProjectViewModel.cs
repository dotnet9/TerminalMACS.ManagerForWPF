using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Nupkg;
using RPA.Interfaces.Project;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
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
    public class NewProjectViewModel : ViewModelBase
    {
        public Window Window { get; set; }

        public bool IsImportNupkgWindow { get; set; }

        private IProjectManagerService _projectManagerService;
        public IPackageImportService _packageImportService;

        /// <summary>
        /// Initializes a new instance of the NewProjectViewModel class.
        /// </summary>
        public NewProjectViewModel(IProjectManagerService projectManagerService)
        {
            _projectManagerService = projectManagerService;

            var path = UserKeyValueConfig.GetValue("Project.DefaultCreatePath", AppPathConfig.DefaultProjectsDir);
            Common.MakeSureDirectoryExists(path);

            ProjectPath = path;
            ProjectName = Common.GetValidDirectoryName(ProjectPath, ProjectConstantConfig.ProjectCreateName, "{0}", 1);
        }


        /// <summary>
        /// The <see cref="WindowTitle" /> property's name.
        /// </summary>
        public const string WindowTitlePropertyName = "WindowTitle";

        private string _windowTitleProperty = "新建项目";

        /// <summary>
        /// Sets and gets the WindowTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return _windowTitleProperty;
            }

            set
            {
                if (_windowTitleProperty == value)
                {
                    return;
                }

                _windowTitleProperty = value;
                RaisePropertyChanged(WindowTitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SubTitle" /> property's name.
        /// </summary>
        public const string SubTitlePropertyName = "SubTitle";

        private string _subTitleProperty = "新建空项目";

        /// <summary>
        /// Sets and gets the SubTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SubTitle
        {
            get
            {
                return _subTitleProperty;
            }

            set
            {
                if (_subTitleProperty == value)
                {
                    return;
                }

                _subTitleProperty = value;
                RaisePropertyChanged(SubTitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SubTitleDescription" /> property's name.
        /// </summary>
        public const string SubTitleDescriptionPropertyName = "SubTitleDescription";

        private string _subTitleDescriptionProperty = "新建一个空白的项目";

        /// <summary>
        /// Sets and gets the SubTitleDescription property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SubTitleDescription
        {
            get
            {
                return _subTitleDescriptionProperty;
            }

            set
            {
                if (_subTitleDescriptionProperty == value)
                {
                    return;
                }

                _subTitleDescriptionProperty = value;
                RaisePropertyChanged(SubTitleDescriptionPropertyName);
            }
        }

        private RelayCommand<RoutedEventArgs> _projectNameLoadedCommand;

        /// <summary>
        /// TextBox加载完成后调用
        /// </summary>
        public RelayCommand<RoutedEventArgs> ProjectNameLoadedCommand
        {
            get
            {
                return _projectNameLoadedCommand
                    ?? (_projectNameLoadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        var textBox = (TextBox)p.Source;
                        textBox.Focus();
                        textBox.SelectAll();
                    }));
            }
        }




        /// <summary>
        /// The <see cref="ProjectVersion" /> property's name.
        /// </summary>
        public const string ProjectVersionPropertyName = "ProjectVersion";

        private string _projectVersionProperty = "";

        /// <summary>
        /// 新建项目时会用默认版本
        /// 如果设置了版本会以该版本为准
        /// </summary>
        public string ProjectVersion
        {
            get
            {
                return _projectVersionProperty;
            }

            set
            {
                if (_projectVersionProperty == value)
                {
                    return;
                }

                _projectVersionProperty = value;
                RaisePropertyChanged(ProjectVersionPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsProjectNameCorrect" /> property's name.
        /// </summary>
        public const string IsProjectNameCorrectPropertyName = "IsProjectNameCorrect";

        private bool _isProjectNameCorrectProperty = false;

        /// <summary>
        /// 项目名称是否正确
        /// </summary>
        public bool IsProjectNameCorrect
        {
            get
            {
                return _isProjectNameCorrectProperty;
            }

            set
            {
                if (_isProjectNameCorrectProperty == value)
                {
                    return;
                }

                _isProjectNameCorrectProperty = value;
                RaisePropertyChanged(IsProjectNameCorrectPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ProjectName" /> property's name.
        /// </summary>
        public const string ProjectNamePropertyName = "ProjectName";

        private string _projectNameProperty = "";

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get
            {
                return _projectNameProperty;
            }

            set
            {
                if (_projectNameProperty == value)
                {
                    return;
                }

                _projectNameProperty = value;
                RaisePropertyChanged(ProjectNamePropertyName);

                projectNameValidate(value);
            }
        }

        /// <summary>
        /// The <see cref="ProjectNameValidatedWrongTip" /> property's name.
        /// </summary>
        public const string ProjectNameValidatedWrongTipPropertyName = "ProjectNameValidatedWrongTip";

        private string _projectNameValidatedWrongTipProperty = "";

        /// <summary>
        /// 项目名称非法时的错误提示
        /// </summary>
        public string ProjectNameValidatedWrongTip
        {
            get
            {
                return _projectNameValidatedWrongTipProperty;
            }

            set
            {
                if (_projectNameValidatedWrongTipProperty == value)
                {
                    return;
                }

                _projectNameValidatedWrongTipProperty = value;
                RaisePropertyChanged(ProjectNameValidatedWrongTipPropertyName);
            }
        }

        /// <summary>
        /// 项目名称合法性检测
        /// </summary>
        /// <param name="value">待检测内容</param>
        private void projectNameValidate(string value)
        {
            IsProjectNameCorrect = true;
            if (string.IsNullOrEmpty(value))
            {
                IsProjectNameCorrect = false;
                ProjectNameValidatedWrongTip = "名称不能为空";
            }
            else
            {
                if (value.Contains(@"\") || value.Contains(@"/"))
                {
                    IsProjectNameCorrect = false;
                    ProjectNameValidatedWrongTip = "名称不能有非法字符";
                }
                else
                {
                    if (!CommonNuget.IsValidPackageId(value))
                    {
                        IsProjectNameCorrect = false;
                        ProjectNameValidatedWrongTip = "名称不能有特殊字符，不能用空格、顿号、逗号等符号，或以特殊符号结尾等形式";
                    }
                }

                if (Directory.Exists(ProjectPath + @"\" + ProjectName))
                {
                    IsProjectNameCorrect = false;
                    ProjectNameValidatedWrongTip = "已经存在同名称的项目";
                }
            }
        }

        /// <summary>
        /// The <see cref="IsProjectPathCorrect" /> property's name.
        /// </summary>
        public const string IsProjectPathCorrectPropertyName = "IsProjectPathCorrect";

        private bool _isProjectPathCorrectProperty = false;

        /// <summary>
        /// 项目路径是否正确
        /// </summary>
        public bool IsProjectPathCorrect
        {
            get
            {
                return _isProjectPathCorrectProperty;
            }

            set
            {
                if (_isProjectPathCorrectProperty == value)
                {
                    return;
                }

                _isProjectPathCorrectProperty = value;
                RaisePropertyChanged(IsProjectPathCorrectPropertyName);
            }
        }

        /// <summary>
        /// 此路径为项目创建时所在的目录
        /// </summary>
        public const string ProjectPathPropertyName = "ProjectPath";

        private string _projectPathProperty = "";

        /// <summary>
        /// Sets and gets the ProjectPath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return _projectPathProperty;
            }

            set
            {
                if (_projectPathProperty == value)
                {
                    return;
                }

                _projectPathProperty = value;
                RaisePropertyChanged(ProjectPathPropertyName);

                projectPathValidate(value);
                projectNameValidate(ProjectName);//路径改变了同样要检查名称
            }
        }

        /// <summary>
        /// 项目路径合法性检测
        /// </summary>
        /// <param name="value">待检测的值</param>
        private void projectPathValidate(string value)
        {
            IsProjectPathCorrect = true;
            if (string.IsNullOrEmpty(value))
            {
                IsProjectPathCorrect = false;
                ProjectPathValidatedWrongTip = "位置不能为空";
            }
            else
            {
                if (!Directory.Exists(value))
                {
                    IsProjectPathCorrect = false;
                    ProjectPathValidatedWrongTip = "指定的位置不存在";
                }
            }
        }

        /// <summary>
        /// The <see cref="ProjectPathValidatedWrongTip" /> property's name.
        /// </summary>
        public const string ProjectPathValidatedWrongTipPropertyName = "ProjectPathValidatedWrongTip";

        private string _projectPathValidatedWrongTipProperty = "";

        /// <summary>
        /// 项目路径检测失败时的错误提示信息
        /// </summary>
        public string ProjectPathValidatedWrongTip
        {
            get
            {
                return _projectPathValidatedWrongTipProperty;
            }

            set
            {
                if (_projectPathValidatedWrongTipProperty == value)
                {
                    return;
                }

                _projectPathValidatedWrongTipProperty = value;
                RaisePropertyChanged(ProjectPathValidatedWrongTipPropertyName);
            }
        }

        private RelayCommand _selectProjectPathCommand;

        /// <summary>
        /// 选择项目路径命令
        /// </summary>
        public RelayCommand SelectProjectPathCommand
        {
            get
            {
                return _selectProjectPathCommand
                    ?? (_selectProjectPathCommand = new RelayCommand(
                    () =>
                    {
                        string dst_dir = "";
                        if (CommonDialog.ShowSelectDirDialog("请选择一个位置来新建项目", ref dst_dir))
                        {
                            ProjectPath = dst_dir;
                        }
                    }));
            }
        }

        /// <summary>
        /// The <see cref="ProjectDescription" /> property's name.
        /// </summary>
        public const string ProjectDescriptionPropertyName = "ProjectDescription";

        private string _projectDescriptionProperty = "空白项目";

        /// <summary>
        /// 项目描述信息
        /// </summary>
        public string ProjectDescription
        {
            get
            {
                return _projectDescriptionProperty;
            }

            set
            {
                if (_projectDescriptionProperty == value)
                {
                    return;
                }

                _projectDescriptionProperty = value;
                RaisePropertyChanged(ProjectDescriptionPropertyName);
            }
        }

        private RelayCommand _createProjectCommand;

        /// <summary>
        /// 创建项目命令
        /// </summary>
        public RelayCommand CreateProjectCommand
        {
            get
            {
                return _createProjectCommand
                    ?? (_createProjectCommand = new RelayCommand(
                    () =>
                    {
                        UserKeyValueConfig.SetKeyValue("Project.DefaultCreatePath", ProjectPath);

                        //新建项目目录及文件
                        if (!_projectManagerService.CloseCurrentProject())
                        {
                            return;
                        }

                        Window.Hide();

                        var projectConfigFileAtPath = Path.Combine(ProjectPath, ProjectName);
                        if (IsImportNupkgWindow)
                        {

                            if (!_packageImportService.ExtractToDirectory(projectConfigFileAtPath))
                            {
                                Window.Close();
                                return;
                            }

                            _projectManagerService.UpdateCurrentProjectConfigFilePath(Path.Combine(projectConfigFileAtPath
                                , ProjectConstantConfig.ProjectConfigFileNameWithSuffix));

                            _projectManagerService.CurrentProjectJsonConfig.name = ProjectName;
                            _projectManagerService.CurrentProjectJsonConfig.description = ProjectDescription;
                            _projectManagerService.SaveCurrentProjectJson();
                        }
                        else
                        {
                            _projectManagerService.NewProject(ProjectPath, ProjectName, ProjectDescription, ProjectVersion);
                        }

                        _projectManagerService.OpenProject(_projectManagerService.CurrentProjectConfigFilePath);

                        Window.Close();
                    },
                    () => IsProjectNameCorrect && IsProjectPathCorrect));
            }
        }

        public void SwitchToImportNupkgWindow(IPackageImportService packageImportService)
        {
            IsImportNupkgWindow = true;

            _packageImportService = packageImportService;

            this.WindowTitle = "导入Nupkg包";
            this.SubTitle = "新建项目";
            this.SubTitleDescription = "导入Nupkg包的内容到新项目中";
            this.ProjectName = _packageImportService.GetId();
            this.ProjectDescription = _packageImportService.GetDescription();
            this.ProjectVersion = _packageImportService.GetVersion();
        }




    }
}