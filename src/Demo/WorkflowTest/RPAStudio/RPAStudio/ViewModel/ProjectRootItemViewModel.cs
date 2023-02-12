using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using RPAStudio.Views;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProjectRootItemViewModel : ProjectBaseItemViewModel
    {
        private IServiceLocator _serviceLocator;

        private IProjectManagerService _projectManagerService;
        private IWorkflowStateService _workflowStateService;

        private ProjectViewModel _projectViewModel;
        private MainViewModel _mainViewModel;

        public string ProjectPath { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the ProjectRootItemViewModel class.
        /// </summary>
        public ProjectRootItemViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();
            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();

            _projectViewModel = _serviceLocator.ResolveType<ProjectViewModel>();
            _mainViewModel = _serviceLocator.ResolveType<MainViewModel>();
        }



        /// <summary>
        /// The <see cref="ToolTip" /> property's name.
        /// </summary>
        public const string ToolTipPropertyName = "ToolTip";

        private string _toolTipProperty = "";

        /// <summary>
        /// Sets and gets the ToolTip property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ToolTip
        {
            get
            {
                return _toolTipProperty;
            }

            set
            {
                if (_toolTipProperty == value)
                {
                    return;
                }

                _toolTipProperty = value;
                RaisePropertyChanged(ToolTipPropertyName);
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
                    }));
            }
        }



        private RelayCommand _openDirCommand;

        /// <summary>
        /// Gets the OpenDirCommand.
        /// </summary>
        public RelayCommand OpenDirCommand
        {
            get
            {
                return _openDirCommand
                    ?? (_openDirCommand = new RelayCommand(
                    () =>
                    {
                        Common.LocateDirInExplorer(ProjectPath);
                    }));
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

                        vm.ProjectPath = ProjectPath;
                        vm.FilePath = ProjectPath;
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

                        vm.ProjectPath = ProjectPath;
                        vm.FilePath = ProjectPath;
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

                        vm.ProjectPath = ProjectPath;
                        vm.FilePath = ProjectPath;
                        vm.FileType = NewXamlFileViewModel.enFileType.StateMachine;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }








        private RelayCommand _newFolderCommand;

        /// <summary>
        /// 新建目录
        /// </summary>
        public RelayCommand NewFolderCommand
        {
            get
            {
                return _newFolderCommand
                    ?? (_newFolderCommand = new RelayCommand(
                    () =>
                    {
                        var window = new NewFolderWindow();

                        var vm = window.DataContext as NewFolderViewModel;
                        vm.Path = ProjectPath;
                        vm.FolderName = Common.GetValidDirectoryName(ProjectPath, "新建文件夹");

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }




        private RelayCommand _openProjectSettingsCommand;

        /// <summary>
        /// Gets the OpenProjectSettingsCommand.
        /// </summary>
        public RelayCommand OpenProjectSettingsCommand
        {
            get
            {
                return _openProjectSettingsCommand
                    ?? (_openProjectSettingsCommand = new RelayCommand(
                    () =>
                    {
                        var window = new ProjectSettingsWindow();

                        var vm = window.DataContext as ProjectSettingsViewModel;
                        vm.ProjectName = _projectManagerService.CurrentProjectJsonConfig.name;
                        vm.ProjectDescription = _projectManagerService.CurrentProjectJsonConfig.description;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }



        private RelayCommand _importWorkflowCommand;

        /// <summary>
        /// Gets the ImportWorkflowCommand.
        /// </summary>
        public RelayCommand ImportWorkflowCommand
        {
            get
            {
                return _importWorkflowCommand
                    ?? (_importWorkflowCommand = new RelayCommand(
                    () =>
                    {
                        //导入工作流
                        var fileList = Common.ShowSelectMultiFileDialog(string.Format("工作流文件|*{0}", ProjectConstantConfig.XamlFileExtension), "选择工作流文件进行导入");

                        foreach (var item in fileList)
                        {
                            var sourceFileName = System.IO.Path.GetFileName(item);
                            var sourceFileDir = System.IO.Path.GetDirectoryName(item);
                            var sourcePath = item;
                            var targetPath = Path + @"\" + sourceFileName;
                            if (System.IO.File.Exists(targetPath))
                            {
                                targetPath = Path + @"\" + Common.GetValidFileName(Path, sourceFileName, " - 导入");
                            }

                            System.IO.File.Copy(sourcePath, targetPath, false);
                        }

                        if (fileList.Count > 0)
                        {
                            _projectViewModel.Refresh();
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
                        _mainViewModel.ExportNupkgCommand.Execute(null);
                    }));
            }
        }



    }

}