using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using RPAStudio.Views;
using System;
using System.IO;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProjectDirItemViewModel : ProjectBaseItemViewModel
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private IServiceLocator _serviceLocator;

        private IWorkflowStateService _workflowStateService;
        private IProjectManagerService _projectManagerService;

        private ProjectViewModel _projectViewModel;
        private DocksViewModel _docksViewModel;

        private MainViewModel _mainViewModel;

        public int _removeUnusedScreenshotsCount;
        /// <summary>
        /// Initializes a new instance of the ProjectDirItemViewModel class.
        /// </summary>
        public ProjectDirItemViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();
            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();

            _projectViewModel = _serviceLocator.ResolveType<ProjectViewModel>();
            _docksViewModel = _serviceLocator.ResolveType<DocksViewModel>();

            _mainViewModel = _serviceLocator.ResolveType<MainViewModel>();
        }






        /// <summary>
        /// The <see cref="IsScreenshots" /> property's name.
        /// </summary>
        public const string IsScreenshotsPropertyName = "IsScreenshots";

        private bool _isScreenshotsProperty = false;

        /// <summary>
        /// Sets and gets the IsScreenshots property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsScreenshots
        {
            get
            {
                return _isScreenshotsProperty;
            }

            set
            {
                if (_isScreenshotsProperty == value)
                {
                    return;
                }

                _isScreenshotsProperty = value;
                RaisePropertyChanged(IsScreenshotsPropertyName);
            }
        }





        private RelayCommand _openDirCommand;

        /// <summary>
        /// 打开所在目录
        /// </summary>
        public RelayCommand OpenDirCommand
        {
            get
            {
                return _openDirCommand
                    ?? (_openDirCommand = new RelayCommand(
                    () =>
                    {
                        Common.LocateDirInExplorer(Path);
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

                        vm.ProjectPath = _projectManagerService.CurrentProjectPath;
                        vm.FilePath = Path;
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
                        vm.FilePath = Path;
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
                        vm.FilePath = Path;
                        vm.FileType = NewXamlFileViewModel.enFileType.StateMachine;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }




        private RelayCommand _renameDirCommand;

        /// <summary>
        /// 重命名文件夹
        /// </summary>
        public RelayCommand RenameDirCommand
        {
            get
            {
                return _renameDirCommand
                    ?? (_renameDirCommand = new RelayCommand(
                    () =>
                    {
                        //重命名目录
                        var window = new RenameWindow();
                        var vm = window.DataContext as RenameViewModel;

                        vm.Path = Path;
                        vm.Dir = System.IO.Path.GetDirectoryName(Path);
                        vm.SrcName = Name;
                        vm.DstName = Common.GetValidDirectoryName(vm.Dir, Common.GetDirectoryNameWithoutSuffixFormat(vm.SrcName));
                        vm.IsDirectory = true;

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }


        private RelayCommand _deleteDirCommand;

        /// <summary>
        /// 删除目录
        /// </summary>
        public RelayCommand DeleteDirCommand
        {
            get
            {
                return _deleteDirCommand
                    ?? (_deleteDirCommand = new RelayCommand(
                    () =>
                    {
                        //删除目录
                        bool ret = CommonMessageBox.ShowQuestion(string.Format("确认删除目录\"{0}\"和它的所有内容吗？", Path));

                        if (ret)
                        {
                            Common.DeleteDir(Path);

                            _projectViewModel.OnDeleteDir(Path);

                            _docksViewModel.OnDeleteDir(Path);
                        }
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
                        vm.Path = Path;
                        vm.FolderName = Common.GetValidDirectoryName(Path, "新建文件夹");

                        CommonWindow.ShowDialog(window);
                    },
                    () => !_workflowStateService.IsRunningOrDebugging));
            }
        }


        /// <summary>
        /// 检测截图是否被引用
        /// </summary>
        /// <param name="item">条目</param>
        /// <param name="param">参数</param>
        /// <returns>是否找到</returns>
        private bool CheckScreenshotsImage(object item, object param)
        {
            if (item is FileInfo)
            {
                var fi_img = param as FileInfo;
                var fi_xaml = item as FileInfo;
                if (fi_xaml.Extension.ToLower() == ProjectConstantConfig.XamlFileExtension)
                {
                    //判断xaml对应的文件里能否找到fi_img的文件名，找到了说明被引用了，直接返回true
                    if (Common.IsStringInFile(fi_xaml.FullName, "\"" + fi_img.Name + "\""))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 遍历项目使用的所有截图
        /// </summary>
        /// <param name="item">条目</param>
        /// <param name="param">参数</param>
        /// <returns>是否继续遍历</returns>
        private bool EnumScreenshotsImage(object item, object param)
        {
            if (item is FileInfo)
            {
                bool contains = Common.DirectoryChildrenForEach(new DirectoryInfo(_projectManagerService.CurrentProjectPath), CheckScreenshotsImage, item);
                if (!contains)
                {
                    var fi = item as FileInfo;
                    try
                    {
                        fi.Delete();

                        _removeUnusedScreenshotsCount++;
                    }
                    catch (Exception err)
                    {
                        _logger.Debug(err);
                    }

                }
            }

            return false;
        }


        private RelayCommand _removeUnusedScreenshotsCommand;

        /// <summary>
        /// Gets the RemoveUnusedScreenshotsCommand.
        /// </summary>
        public RelayCommand RemoveUnusedScreenshotsCommand
        {
            get
            {
                return _removeUnusedScreenshotsCommand
                    ?? (_removeUnusedScreenshotsCommand = new RelayCommand(
                    () =>
                    {
                        _mainViewModel.SaveAllCommand.Execute(null);

                        _removeUnusedScreenshotsCount = 0;

                        Common.DirectoryChildrenForEach(new DirectoryInfo(Path), EnumScreenshotsImage);

                        if (_removeUnusedScreenshotsCount == 0)
                        {
                            CommonMessageBox.ShowInformation("找不到需要清理的未使用的截图");
                        }
                        else
                        {
                            _projectViewModel.Refresh();
                            CommonMessageBox.ShowInformation(string.Format("{0}个未使用的截图已经被成功移除", _removeUnusedScreenshotsCount));
                        }
                    }));
            }
        }


    }
}