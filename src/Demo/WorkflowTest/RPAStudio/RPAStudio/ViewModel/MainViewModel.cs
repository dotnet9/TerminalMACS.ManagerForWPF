using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using RPA.Interfaces.Service;
using System.Windows;
using System;
using RPA.Shared.Configs;
using RPA.Interfaces.Workflow;
using RPA.Interfaces.Project;
using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using RPA.Interfaces.Activities;
using System.ComponentModel;
using RPAStudio.Views;
using System.IO;
using RPA.Interfaces.Nupkg;
using RPA.Shared.Extensions;

namespace RPAStudio.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private const string _rpaTitleFlag = "RPA机器人流程自动化平台（教学版）";

        public Window View;

        private DocksViewModel _docksViewModel;
        private ProjectViewModel _projectViewModel;
        private OutputViewModel _outputViewModel;

        private IServiceLocator _serviceLocator;
        private IWorkflowStateService _workflowStateService;
        private IProjectManagerService _projectManagerService;
        private IRecentProjectsConfigService _recentProjectsConfigService;

        private IActivitiesServiceProxy _activitiesServiceProxy;
        private IWorkflowBreakpointsServiceProxy _workflowBreakpointsServiceProxy;

        private IWorkflowRunService _currentWorkflowRunService;
        public IWorkflowDebugService CurrentWorkflowDebugService { get; set; }

        public event EventHandler ClosingEvent;

        /// <summary>
        /// 速度类型
        /// </summary>
        public enum enSlowStepSpeed
        {
            Off,//关闭
            One,//1x
            Two,//2x
            Three,//3x
            Four//4x
        }

        public MainViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _docksViewModel = _serviceLocator.ResolveType<DocksViewModel>();
            _projectViewModel = _serviceLocator.ResolveType<ProjectViewModel>();
            _outputViewModel = _serviceLocator.ResolveType<OutputViewModel>();

            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();
            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();
            _recentProjectsConfigService = _serviceLocator.ResolveType<IRecentProjectsConfigService>();

            _projectManagerService.ProjectLoadingBeginEvent += _projectManagerService_ProjectLoadingBeginEvent;
            _projectManagerService.ProjectLoadingEndEvent += _projectManagerService_ProjectLoadingEndEvent;
            _projectManagerService.ProjectLoadingExceptionEvent += _projectManagerService_ProjectLoadingExceptionEvent;

            _projectManagerService.ProjectOpenEvent += _projectManagerService_ProjectOpenEvent;
            _projectManagerService.ProjectCloseEvent += _projectManagerService_ProjectCloseEvent;

            _projectManagerService.ProjectPreviewOpenEvent += _projectManagerService_ProjectPreviewOpenEvent;
            _projectManagerService.ProjectPreviewCloseEvent += _projectManagerService_ProjectPreviewCloseEvent;

            _workflowStateService.BeginRunEvent += _workflowStateService_BeginRunEvent;
            _workflowStateService.EndRunEvent += _workflowStateService_EndRunEvent;

            _workflowStateService.BeginDebugEvent += _workflowStateService_BeginDebugEvent;
            _workflowStateService.EndDebugEvent += _workflowStateService_EndDebugEvent;

            SharedObject.Instance.OutputEvent -= Instance_OutputEvent;
            SharedObject.Instance.OutputEvent += Instance_OutputEvent;
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
                        View = (Window)p.Source;

                        OnLoaded();
                    }));
            }
        }


        private void OnLoaded()
        {
            IsShowHomePage = true;//启动后显示主页
            IsHomePageCanClose = false;//主页的回退按钮隐藏，同时ESC也无法关闭主页
        }


        /// <summary>
        /// The <see cref="IsShowHomePage" /> property's name.
        /// </summary>
        public const string IsShowHomePagePropertyName = "IsShowHomePage";

        private bool _isShowHomePageProperty = false;

        /// <summary>
        /// Sets and gets the IsShowHomePage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsShowHomePage
        {
            get
            {
                return _isShowHomePageProperty;
            }

            set
            {
                if (_isShowHomePageProperty == value)
                {
                    return;
                }

                _isShowHomePageProperty = value;
                RaisePropertyChanged(IsShowHomePagePropertyName);


                _docksViewModel.IsAppDomainViewsVisible = !value;
            }
        }



        /// <summary>
        /// The <see cref="IsHomePageCanClose" /> property's name.
        /// </summary>
        public const string IsHomePageCanClosePropertyName = "IsHomePageCanClose";

        private bool _isHomePageCanCloseProperty = false;

        /// <summary>
        /// Sets and gets the IsHomePageCanClose property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsHomePageCanClose
        {
            get
            {
                return _isHomePageCanCloseProperty;
            }

            set
            {
                if (_isHomePageCanCloseProperty == value)
                {
                    return;
                }

                _isHomePageCanCloseProperty = value;
                RaisePropertyChanged(IsHomePageCanClosePropertyName);
            }
        }




        /// <summary>
        /// The <see cref="IsLoading" /> property's name.
        /// </summary>
        public const string IsLoadingPropertyName = "IsLoading";

        private bool _isLoadingProperty = false;

        /// <summary>
        /// Sets and gets the IsLoading property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return _isLoadingProperty;
            }

            set
            {
                if (_isLoadingProperty == value)
                {
                    return;
                }

                _isLoadingProperty = value;
                RaisePropertyChanged(IsLoadingPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="ApplicationName" /> property's name.
        /// </summary>
        public const string ApplicationNamePropertyName = "ApplicationName";

        private string _applicationNameProperty = _rpaTitleFlag;

        /// <summary>
        /// Sets and gets the ApplicationName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return _applicationNameProperty;
            }

            set
            {
                if (_applicationNameProperty == value)
                {
                    return;
                }

                _applicationNameProperty = value;
                RaisePropertyChanged(ApplicationNamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ProjectName" /> property's name.
        /// </summary>
        public const string ProjectNamePropertyName = "ProjectName";

        private string _projectNameProperty = "";

        /// <summary>
        /// Sets and gets the ProjectName property.
        /// Changes to that property's value raise the PropertyChanged event. 
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

                if (string.IsNullOrEmpty(value))
                {
                    ApplicationName = $"{_rpaTitleFlag}";
                }
                else
                {
                    ApplicationName = $"{value} - {_rpaTitleFlag}";
                }

            }
        }


        /// <summary>
        /// The <see cref="IsProjectOpened" /> property's name.
        /// </summary>
        public const string IsProjectOpenedPropertyName = "IsProjectOpened";

        private bool _isProjectOpenedProperty = false;

        /// <summary>
        /// Sets and gets the IsProjectOpened property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsProjectOpened
        {
            get
            {
                return _isProjectOpenedProperty;
            }

            set
            {
                if (_isProjectOpenedProperty == value)
                {
                    return;
                }

                _isProjectOpenedProperty = value;
                RaisePropertyChanged(IsProjectOpenedPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsShowDebug" /> property's name.
        /// </summary>
        public const string IsShowDebugPropertyName = "IsShowDebug";

        private bool _isShowDebugProperty = true;

        /// <summary>
        /// Sets and gets the IsShowDebug property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsShowDebug
        {
            get
            {
                return _isShowDebugProperty;
            }

            set
            {
                if (_isShowDebugProperty == value)
                {
                    return;
                }

                _isShowDebugProperty = value;
                RaisePropertyChanged(IsShowDebugPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="IsShowContinue" /> property's name.
        /// </summary>
        public const string IsShowContinuePropertyName = "IsShowContinue";

        private bool _isShowContinueProperty = false;

        /// <summary>
        /// Sets and gets the IsShowContinue property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsShowContinue
        {
            get
            {
                return _isShowContinueProperty;
            }

            set
            {
                if (_isShowContinueProperty == value)
                {
                    return;
                }

                _isShowContinueProperty = value;
                RaisePropertyChanged(IsShowContinuePropertyName);
            }
        }




        private void _projectManagerService_ProjectLoadingBeginEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsLoading = true;
            });
        }

        private void _projectManagerService_ProjectLoadingEndEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsLoading = false;

                IsShowHomePage = false;
                IsHomePageCanClose = true;
            });
        }

        private void _projectManagerService_ProjectLoadingExceptionEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsLoading = false;
            });
        }


        private void _projectManagerService_ProjectOpenEvent(object sender, System.EventArgs e)
        {
            _logger.Debug($"打开项目：{_projectManagerService.CurrentProjectPath}");

            var service = sender as IProjectManagerService;

            Common.InvokeAsyncOnUI(() => {

                _activitiesServiceProxy = _projectManagerService.CurrentActivitiesServiceProxy;

                ProjectName = service.CurrentProjectJsonConfig.name;

                IsProjectOpened = true;

                _recentProjectsConfigService.Add(service.CurrentProjectConfigFilePath);

                _docksViewModel.View.DebugToolWindow.Close();//默认关闭调试视图
            });

            SharedObject.Instance.ProjectPath = service.CurrentProjectPath;
        }

        private void _projectManagerService_ProjectCloseEvent(object sender, System.EventArgs e)
        {
            _logger.Debug($"关闭项目：{_projectManagerService.CurrentProjectPath}");

            Common.InvokeAsyncOnUI(() => {
                ProjectName = "";

                IsProjectOpened = false;

                //关闭项目后，回到开始页，并且不允许出现返回按钮，不能按ESC返回
                IsShowHomePage = true;//启动后显示主页
                IsHomePageCanClose = false;//主页的回退按钮隐藏，同时ESC也无法关闭主页
            });
        }


        private void _projectManagerService_ProjectPreviewOpenEvent(object sender, EventArgs e)
        {
            _workflowBreakpointsServiceProxy = _serviceLocator.ResolveType<IWorkflowBreakpointsServiceProxy>();
            _workflowBreakpointsServiceProxy.LoadBreakpoints();
        }

        private void _projectManagerService_ProjectPreviewCloseEvent(object sender, CancelEventArgs e)
        {
            _workflowBreakpointsServiceProxy.SaveBreakpoints();
        }

        private void _workflowStateService_BeginRunEvent(object sender, System.EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                _outputViewModel.ClearAllCommand.Execute(null);

                CommonWindow.ShowMainWindowMinimized();
            });
        }

        private void _workflowStateService_EndRunEvent(object sender, System.EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                if (CommonWindow.IsMinimized)
                {
                    CommonWindow.ShowMainWindowNormal();
                }
            });
        }

        private void _workflowStateService_BeginDebugEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                IsShowDebug = false;
                IsShowContinue = true;

                _outputViewModel.ClearAllCommand.Execute(null);

                _docksViewModel.View.DebugToolWindow.Activate();
            });
        }

        private void _workflowStateService_EndDebugEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                IsShowDebug = true;
                IsShowContinue = false;

                _workflowStateService.IsDebuggingPaused = false;//复原调试是否暂停的标志

                _docksViewModel.View.DebugToolWindow.Close();

                if(_docksViewModel.SelectedDocument is DesignerDocumentViewModel)
                {
                    var designerDoc = _docksViewModel.SelectedDocument as DesignerDocumentViewModel;
                    designerDoc.HideCurrentDebugArrow();//隐藏可能的调试标志显示
                }
                
            });
        }


        private void Instance_OutputEvent(SharedObject.enOutputType type, string msg, string msgDetails = "")
        {
            LogToOutputWindow(type, msg, msgDetails);
        }

        private void LogToOutputWindow(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            var logInfo = string.Format("LogToOutputWindow：type={0},msg={1},msgDetails={2}", type.ToString(), msg, msgDetails);
            switch (type)
            {
                case SharedObject.enOutputType.Trace:
                    _logger.Trace(logInfo);
                    break;
                case SharedObject.enOutputType.Information:
                    _logger.Info(logInfo);
                    break;
                case SharedObject.enOutputType.Warning:
                    _logger.Warn(logInfo);
                    break;
                case SharedObject.enOutputType.Error:
                    _logger.Error(logInfo);
                    break;
                default:
                    _logger.Info(logInfo);
                    break;
            }
            
            //输出到输出窗口
            Common.InvokeAsyncOnUI(() => {
                _serviceLocator.ResolveType<OutputViewModel>().Log(type, msg, msgDetails);
            });
        }




        private RelayCommand _openProjectCommand;

        /// <summary>
        /// Gets the OpenProjectCommand.
        /// </summary>
        public RelayCommand OpenProjectCommand
        {
            get
            {
                return _openProjectCommand
                    ?? (_openProjectCommand = new RelayCommand(
                    () =>
                    {
                        var fileOpened = Common.ShowSelectSingleFileDialog($"工作流项目文件|{ProjectConstantConfig.ProjectConfigFileNameWithSuffix}|Nupkg包文件|*.nupkg");
                        string fileExt = Path.GetExtension(fileOpened);
                        if (fileExt.ToLower() == ".nupkg")
                        {
                            var nupkgFilePath = fileOpened;

                            //类似于新建项目，在默认项目路径进行创建，以Nupkg的文件名创建目录，如果目录重命，提示用户
                            //弹出新建项目对话框
                            var packageImportService = _serviceLocator.ResolveType<IPackageImportService>();
                            if (packageImportService.Init(nupkgFilePath))
                            {
                                var window = new NewProjectWindow();
                                var vm = window.DataContext as NewProjectViewModel;
                                vm.Window = window;
                                vm.SwitchToImportNupkgWindow(packageImportService);
                                CommonWindow.ShowDialog(window);
                            }
                            else
                            {
                                CommonMessageBox.ShowError("该Nupkg包无法正常读取，可能包格式非法！");
                            }
                        }
                        else if (fileExt.ToLower() == ProjectConstantConfig.ProjectConfigFileExtension)
                        {
                            var projectConfigFilePath = fileOpened;
                            if (_projectManagerService.IsAlreadyOpened(projectConfigFilePath))
                            {
                                return;
                            }

                            if (!string.IsNullOrEmpty(projectConfigFilePath))
                            {
                                if (_projectManagerService.CloseCurrentProject())
                                {
                                    _projectManagerService.OpenProject(projectConfigFilePath);
                                }
                            }
                        }
                    }));
            }
        }




        private RelayCommand _closeProjectCommand;

        /// <summary>
        /// Gets the CloseProjectCommand.
        /// </summary>
        public RelayCommand CloseProjectCommand
        {
            get
            {
                return _closeProjectCommand
                    ?? (_closeProjectCommand = new RelayCommand(
                    () =>
                    {
                        _projectManagerService.CloseCurrentProject();
                    },
                    () => IsProjectOpened));
            }
        }



        private RelayCommand _newSequenceCommand;

        /// <summary>
        /// Gets the NewSequenceCommand.
        /// </summary>
        public RelayCommand NewSequenceCommand
        {
            get
            {
                return _newSequenceCommand
                    ?? (_newSequenceCommand = new RelayCommand(
                    () =>
                    {
                        var window = new NewXamlFileWindow();
                        var vm = window.DataContext as NewXamlFileViewModel;

                        vm.ProjectPath = _projectManagerService.CurrentProjectPath;
                        vm.FilePath = _projectManagerService.CurrentProjectPath;
                        vm.FileType = NewXamlFileViewModel.enFileType.Sequence;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }





        private RelayCommand _newFlowchartCommand;

        /// <summary>
        /// Gets the NewFlowchartCommand.
        /// </summary>
        public RelayCommand NewFlowchartCommand
        {
            get
            {
                return _newFlowchartCommand
                    ?? (_newFlowchartCommand = new RelayCommand(
                    () =>
                    {
                        var window = new NewXamlFileWindow();
                        var vm = window.DataContext as NewXamlFileViewModel;

                        vm.ProjectPath = _projectManagerService.CurrentProjectPath;
                        vm.FilePath = _projectManagerService.CurrentProjectPath;
                        vm.FileType = NewXamlFileViewModel.enFileType.Flowchart;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }



        private RelayCommand _newStateMachineCommand;

        /// <summary>
        /// Gets the NewStateMachineCommand.
        /// </summary>
        public RelayCommand NewStateMachineCommand
        {
            get
            {
                return _newStateMachineCommand
                    ?? (_newStateMachineCommand = new RelayCommand(
                    () =>
                    {
                        var window = new NewXamlFileWindow();
                        var vm = window.DataContext as NewXamlFileViewModel;

                        vm.ProjectPath = _projectManagerService.CurrentProjectPath;
                        vm.FilePath = _projectManagerService.CurrentProjectPath;
                        vm.FileType = NewXamlFileViewModel.enFileType.StateMachine;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }


        private RelayCommand _runWorkflowCommand;

        /// <summary>
        /// Gets the RunWorkflowCommand.
        /// </summary>
        public RelayCommand RunWorkflowCommand
        {
            get
            {
                return _runWorkflowCommand
                    ?? (_runWorkflowCommand = new RelayCommand(
                    () =>
                    {
                        //全部保存
                        SaveAllCommand.Execute(null);

                        _currentWorkflowRunService = _serviceLocator.ResolveType<IWorkflowRunService>();
                        _currentWorkflowRunService.Init(_docksViewModel.SelectedDocument.Path
                            , _projectManagerService.CurrentActivitiesDllLoadFrom, _projectManagerService.CurrentDependentAssemblies);

                        _currentWorkflowRunService.Run();

                    },
                    () => _docksViewModel.SelectedDocument is DesignerDocumentViewModel
                    && !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                    && !_workflowStateService.IsRunningOrDebugging));
            }
        }


        private RelayCommand _stopWorkflowCommand;

        /// <summary>
        /// Gets the StopWorkflowCommand.
        /// </summary>
        public RelayCommand StopWorkflowCommand
        {
            get
            {
                return _stopWorkflowCommand
                    ?? (_stopWorkflowCommand = new RelayCommand(
                    () =>
                    {
                        if (_workflowStateService.IsRunning)
                        {
                            _currentWorkflowRunService.Stop();
                        }

                        if (_workflowStateService.IsDebugging)
                        {
                            if (CurrentWorkflowDebugService == null)
                            {
                                CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();
                            }

                            CurrentWorkflowDebugService.Stop();
                        }
                    },
                    () => _workflowStateService.IsRunningOrDebugging));
            }
        }



        private RelayCommand _debugWorkflowCommand;

        /// <summary>
        /// Gets the DebugWorkflowCommand.
        /// </summary>
        public RelayCommand DebugWorkflowCommand
        {
            get
            {
                return _debugWorkflowCommand
                    ?? (_debugWorkflowCommand = new RelayCommand(
                    () =>
                    {
#if ENABLE_AUTHORIZATION_CHECK
                        _authorizationService.DoAuthorizationCheck();
#endif

                        //全部保存
                        SaveAllCommand.Execute(null);

                        CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();

                        var doc = _docksViewModel.SelectedDocument as DesignerDocumentViewModel;
                        CurrentWorkflowDebugService.Init(doc.GetWorkflowDesignerServiceProxy(), _docksViewModel.SelectedDocument.Path
                            , _projectManagerService.CurrentActivitiesDllLoadFrom, _projectManagerService.CurrentDependentAssemblies);

                        CurrentWorkflowDebugService.SetNextOperate(enOperate.Null);//避免上次操作遗留下来的影响
                        CurrentWorkflowDebugService.Debug();
                    },
                    () => _docksViewModel.SelectedDocument is DesignerDocumentViewModel
                    && !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                    && !_workflowStateService.IsRunningOrDebugging));
            }
        }



        private RelayCommand _continueWorkflowCommand;

        /// <summary>
        /// Gets the ContinueWorkflowCommand.
        /// </summary>
        public RelayCommand ContinueWorkflowCommand
        {
            get
            {
                return _continueWorkflowCommand
                    ?? (_continueWorkflowCommand = new RelayCommand(
                    () =>
                    {
                        if (CurrentWorkflowDebugService == null)
                        {
                            CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();
                        }

                        CurrentWorkflowDebugService.Continue();
                    },
                    () => _workflowStateService.IsDebuggingPaused));
            }
        }



        private RelayCommand _toggleBreakpointCommand;

        /// <summary>
        /// Gets the ToggleBreakpointCommand.
        /// </summary>
        public RelayCommand ToggleBreakpointCommand
        {
            get
            {
                return _toggleBreakpointCommand
                    ?? (_toggleBreakpointCommand = new RelayCommand(
                    () =>
                    {
                        _workflowBreakpointsServiceProxy.ToggleBreakpoint(_docksViewModel.SelectedDocument?.Path);
                    },
                    () => _docksViewModel.SelectedDocument is DesignerDocumentViewModel
                    && !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                    ));
            }
        }


        private RelayCommand _removeAllBreakpointsCommand;

        /// <summary>
        /// Gets the RemoveAllBreakpointsCommand.
        /// </summary>
        public RelayCommand RemoveAllBreakpointsCommand
        {
            get
            {
                return _removeAllBreakpointsCommand
                    ?? (_removeAllBreakpointsCommand = new RelayCommand(
                    () =>
                    {
                        _workflowBreakpointsServiceProxy.RemoveAllBreakpoints(_docksViewModel.SelectedDocument?.Path);
                    },
                    () => _docksViewModel.SelectedDocument is DesignerDocumentViewModel
                    && !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                    ));
            }
        }



        private RelayCommand _breakCommand;

        /// <summary>
        /// Gets the BreakCommand.
        /// </summary>
        public RelayCommand BreakCommand
        {
            get
            {
                return _breakCommand
                    ?? (_breakCommand = new RelayCommand(
                    () =>
                    {
                        if (CurrentWorkflowDebugService == null)
                        {
                            CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();
                        }

                        CurrentWorkflowDebugService.Break();
                    },
                    () => _workflowStateService.IsDebugging && !_workflowStateService.IsDebuggingPaused));
            }
        }



        private RelayCommand _stepIntoCommand;

        /// <summary>
        /// Gets the StepIntoCommand.
        /// </summary>
        public RelayCommand StepIntoCommand
        {
            get
            {
                return _stepIntoCommand
                    ?? (_stepIntoCommand = new RelayCommand(
                    () =>
                    {
                        if (!_workflowStateService.IsRunningOrDebugging)
                        {
                            SaveAllCommand.Execute(null);

                            CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();

                            var doc = _docksViewModel.SelectedDocument as DesignerDocumentViewModel;
                            CurrentWorkflowDebugService.Init(doc.GetWorkflowDesignerServiceProxy(), _docksViewModel.SelectedDocument.Path
                                , _projectManagerService.CurrentActivitiesDllLoadFrom, _projectManagerService.CurrentDependentAssemblies);
                            CurrentWorkflowDebugService.SetNextOperate(enOperate.StepInto);

                            CurrentWorkflowDebugService.Debug();
                        }
                        else
                        {
                            if (CurrentWorkflowDebugService == null)
                            {
                                CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();
                            }

                            CurrentWorkflowDebugService.Continue(enOperate.StepInto);
                        }
                    },
                    () => !_workflowStateService.IsRunning
                    && _docksViewModel.SelectedDocument is DesignerDocumentViewModel
                    && !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                    && (!_workflowStateService.IsDebugging || (_workflowStateService.IsDebugging && _workflowStateService.IsDebuggingPaused))
                    ));
            }
        }



        private RelayCommand _stepOverCommand;

        /// <summary>
        /// Gets the StepOverCommand.
        /// </summary>
        public RelayCommand StepOverCommand
        {
            get
            {
                return _stepOverCommand
                    ?? (_stepOverCommand = new RelayCommand(
                    () =>
                    {
                        if (!_workflowStateService.IsRunningOrDebugging)
                        {
                            SaveAllCommand.Execute(null);

                            CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();

                            var doc = _docksViewModel.SelectedDocument as DesignerDocumentViewModel;
                            CurrentWorkflowDebugService.Init(doc.GetWorkflowDesignerServiceProxy(), _docksViewModel.SelectedDocument.Path
                                , _projectManagerService.CurrentActivitiesDllLoadFrom, _projectManagerService.CurrentDependentAssemblies);
                            CurrentWorkflowDebugService.SetNextOperate(enOperate.StepOver);

                            CurrentWorkflowDebugService.Debug();
                        }
                        else
                        {
                            if (CurrentWorkflowDebugService == null)
                            {
                                CurrentWorkflowDebugService = _serviceLocator.ResolveType<IWorkflowDebugService>();
                            }

                            CurrentWorkflowDebugService.Continue(enOperate.StepOver);
                        }
                    },
                    () => !_workflowStateService.IsRunning
                    && _docksViewModel.SelectedDocument is DesignerDocumentViewModel
                    && !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                   && (!_workflowStateService.IsDebugging || (_workflowStateService.IsDebugging && _workflowStateService.IsDebuggingPaused))
                    ));
            }
        }



        /// <summary>
        /// The <see cref="SlowStepSpeed" /> property's name.
        /// </summary>
        public const string SlowStepSpeedPropertyName = "SlowStepSpeed";

        private enSlowStepSpeed _slowStepSpeedProperty = enSlowStepSpeed.Off;

        /// <summary>
        /// 慢速运行的速度
        /// </summary>
        public enSlowStepSpeed SlowStepSpeed
        {
            get
            {
                return _slowStepSpeedProperty;
            }

            set
            {
                if (_slowStepSpeedProperty == value)
                {
                    return;
                }

                _slowStepSpeedProperty = value;
                RaisePropertyChanged(SlowStepSpeedPropertyName);
            }
        }



        private RelayCommand _slowStepCommand;

        /// <summary>
        /// 慢速运行
        /// </summary>
        public RelayCommand SlowStepCommand
        {
            get
            {
                return _slowStepCommand
                    ?? (_slowStepCommand = new RelayCommand(
                    () =>
                    {
                        switch (SlowStepSpeed)
                        {
                            case enSlowStepSpeed.Off:
                                SlowStepSpeed = enSlowStepSpeed.One;
                                _workflowStateService.SpeedType = enSpeed.One;
                                break;
                            case enSlowStepSpeed.One:
                                SlowStepSpeed = enSlowStepSpeed.Two;
                                _workflowStateService.SpeedType = enSpeed.Two;
                                break;
                            case enSlowStepSpeed.Two:
                                SlowStepSpeed = enSlowStepSpeed.Three;
                                _workflowStateService.SpeedType = enSpeed.Three;
                                break;
                            case enSlowStepSpeed.Three:
                                SlowStepSpeed = enSlowStepSpeed.Four;
                                _workflowStateService.SpeedType = enSpeed.Four;
                                break;
                            case enSlowStepSpeed.Four:
                                SlowStepSpeed = enSlowStepSpeed.Off;
                                _workflowStateService.SpeedType = enSpeed.Off;
                                break;
                            default:
                                break;
                        }

                        _workflowStateService.RaiseUpdateSlowStepSpeedEvent(_workflowStateService.SpeedType);
                    }));
            }
        }



        private RelayCommand _validateWorkflowCommand;

        /// <summary>
        /// 校验工作流有效性
        /// </summary>
        public RelayCommand ValidateWorkflowCommand
        {
            get
            {
                return _validateWorkflowCommand
                    ?? (_validateWorkflowCommand = new RelayCommand(
                    () =>
                    {
                        if (_activitiesServiceProxy.IsXamlStringValid(_docksViewModel.SelectedDocument.XamlText))
                        {
                            CommonMessageBox.ShowInformation("工作流校验正确");
                        }
                        else
                        {
                            CommonMessageBox.ShowError("工作流校验错误，请检查参数配置");
                        }
                    },
                    () => !string.IsNullOrEmpty(_docksViewModel.SelectedDocument?.Path)
                    ));
            }
        }



        /// <summary>
        /// The <see cref="IsLogActivities" /> property's name.
        /// </summary>
        public const string IsLogActivitiesPropertyName = "IsLogActivities";

        private bool _isLogActivitiesProperty = false;

        /// <summary>
        /// 是否记录活动
        /// </summary>
        public bool IsLogActivities
        {
            get
            {
                return _isLogActivitiesProperty;
            }

            set
            {
                if (_isLogActivitiesProperty == value)
                {
                    return;
                }

                _isLogActivitiesProperty = value;
                RaisePropertyChanged(IsLogActivitiesPropertyName);

                _workflowStateService.IsLogActivities = value;
                _workflowStateService.RaiseUpdateIsLogActivitiesEvent(_workflowStateService.IsLogActivities);
            }
        }



        private RelayCommand _openLogsCommand;

        /// <summary>
        /// Gets the OpenLogsCommand.
        /// </summary>
        public RelayCommand OpenLogsCommand
        {
            get
            {
                return _openLogsCommand
                    ?? (_openLogsCommand = new RelayCommand(
                    () =>
                    {
                        Common.LocateDirInExplorer(AppPathConfig.StudioLogsDir);
                    }));
            }
        }


        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(
                    () =>
                    {
                        _docksViewModel.SelectedDocument?.Save();
                    },
                    () => _docksViewModel.SelectedDocument?.CanSave() == true));
            }
        }



        private RelayCommand _saveAsCommand;

        /// <summary>
        /// Gets the SaveAsCommand.
        /// </summary>
        public RelayCommand SaveAsCommand
        {
            get
            {
                return _saveAsCommand
                    ?? (_saveAsCommand = new RelayCommand(
                    () =>
                    {
                        var doc = _docksViewModel.SelectedDocument;

                        string userSelPath;
                        bool ret = Common.ShowSaveAsFileDialog(out userSelPath, doc.Title, ProjectConstantConfig.XamlFileExtension, "工作流文件");

                        if (ret == true)
                        {
                            //保存xaml到文件中
                            var xamlText = doc.XamlText;
                            File.WriteAllText(userSelPath, xamlText);

                            doc.IsDirty = false;
                            doc.Path = userSelPath;
                            doc.ToolTip = doc.Path;
                            doc.Title = Path.GetFileNameWithoutExtension(userSelPath);
                            doc.UpdatePathCrossDomain(doc.Path);

                            //如果另存为的路径在当前项目路径下，则需要刷新项目树视图
                            if (userSelPath.IsSubPathOf(_projectManagerService.CurrentProjectPath))
                            {
                                _projectViewModel.RefreshCommand.Execute(null);
                            }
                        }
                    },
                    () => _docksViewModel.SelectedDocument?.CanSave() == true));
            }
        }

        private RelayCommand _saveAllCommand;

        /// <summary>
        /// Gets the SaveAllCommand.
        /// </summary>
        public RelayCommand SaveAllCommand
        {
            get
            {
                return _saveAllCommand
                    ?? (_saveAllCommand = new RelayCommand(
                    () =>
                    {
                        foreach (var doc in _docksViewModel.Documents)
                        {
                            doc.Save();
                        }
                    }));
            }
        }


        private RelayCommand _exportNupkgCommand;

        /// <summary>
        /// Gets the ExportNupkgCommand.
        /// </summary>
        public RelayCommand ExportNupkgCommand
        {
            get
            {
                return _exportNupkgCommand
                    ?? (_exportNupkgCommand = new RelayCommand(
                    () =>
                    {
                        var window = new ExportWindow();
                        var vm = window.DataContext as ExportViewModel;
                        vm.Window = window;
                        CommonWindow.ShowDialog(window);
                    },
                    () => IsProjectOpened));
            }
        }



        private RelayCommand _cutCommand;

        /// <summary>
        /// Gets the CutCommand.
        /// </summary>
        public RelayCommand CutCommand
        {
            get
            {
                return _cutCommand
                    ?? (_cutCommand = new RelayCommand(
                    () =>
                    {
                        _docksViewModel.SelectedDocument?.Cut();
                    },
                    () => _docksViewModel.SelectedDocument?.CanCut() == true));
            }
        }


        private RelayCommand _copyCommand;

        /// <summary>
        /// Gets the CopyCommand.
        /// </summary>
        public RelayCommand CopyCommand
        {
            get
            {
                return _copyCommand
                    ?? (_copyCommand = new RelayCommand(
                    () =>
                    {
                        _docksViewModel.SelectedDocument?.Copy();
                    },
                    () => _docksViewModel.SelectedDocument?.CanCopy() == true));
            }
        }


        private RelayCommand _pasteCommand;

        /// <summary>
        /// Gets the PasteCommand.
        /// </summary>
        public RelayCommand PasteCommand
        {
            get
            {
                return _pasteCommand
                    ?? (_pasteCommand = new RelayCommand(
                    () =>
                    {
                        _docksViewModel.SelectedDocument?.Paste();
                    },
                    () => _docksViewModel.SelectedDocument?.CanPaste() == true));
            }
        }



        private RelayCommand<CancelEventArgs> _closingCommand;

        /// <summary>
        /// 主窗口正要关闭时触发
        /// </summary>
        public RelayCommand<CancelEventArgs> ClosingCommand
        {
            get
            {
                return _closingCommand
                    ?? (_closingCommand = new RelayCommand<CancelEventArgs>(
                    p =>
                    {
                        //正在加载过程中不允许关闭窗口，否则Adorner容易引发异常报错
                        if (IsLoading)
                        {
                            p.Cancel = true;
                            return;
                        }

                        //关闭主窗口前确认
                        bool bContinueClose = _docksViewModel.CloseAllQuery();

                        if (!bContinueClose)
                        {
                            p.Cancel = true;
                        }
                        else
                        {
                            StopWorkflowCommand.Execute(null);
                            ClosingEvent?.Invoke(this, EventArgs.Empty);
                        }
                    }));
            }
        }



        private RelayCommand _closedCommand;

        /// <summary>
        /// Gets the ClosedCommand.
        /// </summary>
        public RelayCommand ClosedCommand
        {
            get
            {
                return _closedCommand
                    ?? (_closedCommand = new RelayCommand(
                    () =>
                    {

                    }));
            }
        }

        private RelayCommand _debugOrContinueWorkflowCommand;

        /// <summary>
        /// 调试或继续运行命令
        /// </summary>
        public RelayCommand DebugOrContinueWorkflowCommand
        {
            get
            {
                return _debugOrContinueWorkflowCommand
                    ?? (_debugOrContinueWorkflowCommand = new RelayCommand(
                    () =>
                    {
                        DebugWorkflowCommand.Execute(null);
                        ContinueWorkflowCommand.Execute(null);
                    },
                    () => true));
            }
        }


    }
}