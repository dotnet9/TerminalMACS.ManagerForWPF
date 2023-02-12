using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using RPA.Interfaces.Project;
using RPA.Interfaces.Workflow;
using RPA.Shared.Utils;
using System;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProjectSettingsViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 对应的视图
        /// </summary>
        private Window _view;

        private IProjectManagerService _projectManagerService;
        private IWorkflowStateService _workflowStateService;

        private ProjectViewModel _projectViewModel;

        private IRecentProjectsConfigService _recentProjectsConfigService;

        /// <summary>
        /// Initializes a new instance of the ProjectSettingsViewModel class.
        /// </summary>
        public ProjectSettingsViewModel(IProjectManagerService projectManagerService, IRecentProjectsConfigService recentProjectsConfigService
            , IWorkflowStateService workflowStateService, ProjectViewModel projectViewModel)
        {
            _projectManagerService = projectManagerService;
            _recentProjectsConfigService = recentProjectsConfigService;
            _workflowStateService = workflowStateService;

            _projectViewModel = projectViewModel;
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
        /// 项目名称校验错误时的提示信息
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
        /// 项目名称校验
        /// </summary>
        /// <param name="value">待校验的值</param>
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
                            IsProjectNameCorrect = false;
                            ProjectNameValidatedWrongTip = "名称不能有非法字符";
                        }
                    }
                }
            }

            OkCommand.RaiseCanExecuteChanged();
        }




        /// <summary>
        /// The <see cref="ProjectDescription" /> property's name.
        /// </summary>
        public const string ProjectDescriptionPropertyName = "ProjectDescription";

        private string _projectDescriptionProperty = "";

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
                        UpdateProjectJson();

                        _projectViewModel.OnProjectSettingsModify(this);

                        _recentProjectsConfigService.Update(_projectManagerService.CurrentProjectConfigFilePath, ProjectName, ProjectDescription);

                        _view.Close();
                    },
                    () => IsProjectNameCorrect && !_workflowStateService.IsRunningOrDebugging));
            }
        }

        /// <summary>
        /// 更新project.rpa文件
        /// </summary>
        private void UpdateProjectJson()
        {
            _projectManagerService.CurrentProjectJsonConfig.name = ProjectName;
            _projectManagerService.CurrentProjectJsonConfig.description = ProjectDescription;
            //json更新
            _projectManagerService.SaveCurrentProjectJson();
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
                    },
                    () => true));
            }
        }


    }
}