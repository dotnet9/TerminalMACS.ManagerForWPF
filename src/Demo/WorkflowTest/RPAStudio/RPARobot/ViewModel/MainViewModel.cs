using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using NuGet;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using RPARobot.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RPARobot.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;

        private IRobotPathConfigService _robotPathConfigService;

        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 工作流运行管理器
        /// </summary>
        private IRunManagerService _runManagerService { get; set; }

        /// <summary>
        /// 对应的视图
        /// </summary>
        private Window _view { get; set; }


        /// <summary>
        /// Packages目录位置
        /// </summary>
        private string _programDataPackagesDir { get; set; }

        /// <summary>
        /// InstalledPackages目录位置
        /// </summary>
        private string _programDataInstalledPackagesDir { get; set; }

        /// <summary>
        /// 包服务类
        /// </summary>
        private IPackageService _packageService { get; set; }


        private ConcurrentDictionary<string, bool> _packageItemEnableDict = new ConcurrentDictionary<string, bool>();


        private StartupViewModel _startupViewModel;


        public string ProgramDataPackagesDir
        {
            get
            {
                return _programDataPackagesDir;
            }
        }

        public string ProgramDataInstalledPackagesDir
        {
            get
            {
                return _programDataInstalledPackagesDir;
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _runManagerService = _serviceLocator.ResolveType<IRunManagerService>();

            _runManagerService.BeginRunEvent += _runManagerService_BeginRunEvent;
            _runManagerService.EndRunEvent += _runManagerService_EndRunEvent;

            Common.InvokeAsyncOnUI(() =>
            {
                _startupViewModel = _serviceLocator.ResolveType<StartupViewModel>();
                _packageService = _serviceLocator.ResolveType<IPackageService>();

                _robotPathConfigService = _serviceLocator.ResolveType<IRobotPathConfigService>();
                _robotPathConfigService.InitDirs();

                _programDataPackagesDir = _robotPathConfigService.ProgramDataPackagesDir;
                _programDataInstalledPackagesDir = _robotPathConfigService.ProgramDataInstalledPackagesDir;
            });
        }

        private void _runManagerService_BeginRunEvent(object sender, EventArgs e)
        {
            var obj = sender as IRunManagerService;

            //流程运行开始……
            SharedObject.Instance.Output(SharedObject.enOutputType.Trace, "流程运行开始");

            
            Common.InvokeOnUI(() =>
            {
                _view.Hide();

                obj.PackageItem.IsRunning = true;

                IsWorkflowRunning = true;
                WorkflowRunningName = obj.PackageItem.Name;
                WorkflowRunningToolTip = obj.PackageItem.ToolTip;
                WorkflowRunningStatus = "正在运行";
            });
        }

        private void _runManagerService_EndRunEvent(object sender, EventArgs e)
        {
            var obj = sender as IRunManagerService;

            SharedObject.Instance.Output(SharedObject.enOutputType.Trace, "流程运行结束"); //流程运行结束

            Common.InvokeOnUI(() =>
            {
                _view.Show();
                _view.Activate();

                obj.PackageItem.IsRunning = false;

                //由于有可能列表已经刷新，所以需要重置IsRunning状态，为了方便，全部重置
                foreach (var pkg in PackageItems)
                {
                    pkg.IsRunning = false;
                }

                IsWorkflowRunning = false;
                WorkflowRunningName = "";
                WorkflowRunningStatus = "";
            });
        }


        /// <summary>
        /// 刷新所有包列表
        /// </summary>
        public void RefreshAllPackages()
        {
            PackageItems.Clear();

            var repo = PackageRepositoryFactory.Default.CreateRepository(_programDataPackagesDir);
            var pkgList = repo.GetPackages();

            var pkgSet = new SortedSet<string>();
            foreach (var pkg in pkgList)
            {
                //通过set去重
                pkgSet.Add(pkg.Id);
            }

            Dictionary<string, IPackage> installedPkgDict = new Dictionary<string, IPackage>();

            var packageManager = new PackageManager(repo, _programDataInstalledPackagesDir);
            foreach (IPackage pkg in packageManager.LocalRepository.GetPackages())
            {
                installedPkgDict[pkg.Id] = pkg;//记录安装到InstalledPackages目录下的安装包的名称和对应的最大版本号
            }

            foreach (var name in pkgSet)
            {
                try
                {
                    var item = _serviceLocator.ResolveType<PackageItemViewModel>();
                    item.Name = name;

                    var version = repo.FindPackagesById(name).Max(p => p.Version);
                    item.Version = version.ToString();

                    var pkgNameList = repo.FindPackagesById(name);
                    foreach (var i in pkgNameList)
                    {
                        item._versionList.Add(i.Version.ToString());
                    }

                    bool isNeedUpdate = false;
                    if (installedPkgDict.ContainsKey(item.Name))
                    {
                        var installedVer = installedPkgDict[item.Name].Version;
                        if (version > installedVer)
                        {
                            isNeedUpdate = true;
                        }
                    }
                    else
                    {
                        isNeedUpdate = true;
                    }
                    item.IsNeedUpdate = isNeedUpdate;

                    var pkg = repo.FindPackage(name, version);
                    item.Package = pkg;
                    var publishedTime = pkg.Published.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    string toolTip = "名称：{0}\r\n版本：{1}\r\n发布说明：{2}\r\n项目描述：{3}\r\n发布时间：{4}";
                    toolTip = toolTip.Replace(@"\r\n", "\r\n");//换行符修正
                    item.ToolTip = string.Format(toolTip, item.Name, item.Version, pkg.ReleaseNotes, pkg.Description, (publishedTime == null ? "未知" : publishedTime));

                    if (IsWorkflowRunning && item.Name == WorkflowRunningName)
                    {
                        item.IsRunning = true;//如果当前该包工程已经在运行，则要设置IsRunning
                    }

                    PackageItems.Add(item);
                }
                catch (Exception err)
                {
                    _logger.Debug($"获取流程 <{name}> 的信息出现异常，该流程对应的某个版本可能格式有问题，异常详情：" + err.ToString(), _logger);
                }
            }

            doSearch();
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
                        RefreshAllPackages();
                    }));
            }
        }




       
        private RelayCommand _MouseLeftButtonDownCommand;

        /// <summary>
        /// 鼠标左键按下时触发
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
                        _view.DragMove();
                    }));
            }
        }

        private RelayCommand _activatedCommand;

        /// <summary>
        /// 窗体处于激活状态时触发
        /// </summary>
        public RelayCommand ActivatedCommand
        {
            get
            {
                return _activatedCommand
                    ?? (_activatedCommand = new RelayCommand(
                    () =>
                    {
                        RefreshAllPackages();
                    }));
            }
        }



        private RelayCommand<System.ComponentModel.CancelEventArgs> _closingCommand;

        /// <summary>
        /// 窗体正在关闭时触发
        /// </summary>
        public RelayCommand<System.ComponentModel.CancelEventArgs> ClosingCommand
        {
            get
            {
                return _closingCommand
                    ?? (_closingCommand = new RelayCommand<System.ComponentModel.CancelEventArgs>(
                    e =>
                    {
                        e.Cancel = true;//不关闭窗口
                        _view.Hide();
                    }));
            }
        }


        private RelayCommand<DragEventArgs> _dropCommandCommand;

        /// <summary>
        /// Gets the DropCommand.
        /// </summary>
        public RelayCommand<DragEventArgs> DropCommand
        {
            get
            {
                return _dropCommandCommand
                    ?? (_dropCommandCommand = new RelayCommand<DragEventArgs>(
                    e =>
                    {
                        if (e.Data.GetDataPresent(DataFormats.FileDrop))
                        {
                            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                            foreach (string fileFullPath in files)
                            {
                                string extension = System.IO.Path.GetExtension(fileFullPath);
                                if (extension.ToLower() == ".nupkg")
                                {
                                    processImportNupkgFile(fileFullPath);
                                }
                            }
                        }
                    }));
            }
        }

        /// <summary>
        /// 导入nupkg包到本地
        /// </summary>
        /// <param name="file">nupkg包路径</param>
        private bool processImportNupkgFile(string fileFullPath)
        {
            //复制file到PackagesDir

            //首先判断PackagesDir里有没有重名的文件，有的话要提示用户是否覆盖
            var fileName = System.IO.Path.GetFileName(fileFullPath);

            var dstFileFullPath = _programDataPackagesDir + @"\" + fileName;
            if (System.IO.File.Exists(dstFileFullPath))
            {
                var ret = MessageBox.Show(App.Current.MainWindow, $"目标目录“{_programDataPackagesDir}”存在同名文件“{fileName}”，是否替换？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (ret != MessageBoxResult.Yes)
                {
                    return false;
                }
            }

            try
            {
                System.IO.File.Copy(fileFullPath, dstFileFullPath, true);
                RefreshCommand.Execute(null);
                CommonMessageBox.ShowInformation($"导入“{fileName}”成功");
            }
            catch (Exception err)
            {
                _logger.Debug(err);
                CommonMessageBox.ShowInformation($"导入“{fileName}”发生异常！");
                return false;
            }

            return true;
        }

        private RelayCommand _refreshCommand;

        /// <summary>
        /// 刷新
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = new RelayCommand(
                    () =>
                    {
                        RefreshAllPackages();
                    }));
            }
        }


        private RelayCommand _viewLogsCommand;

        /// <summary>
        /// 查看日志
        /// </summary>
        public RelayCommand ViewLogsCommand
        {
            get
            {
                return _viewLogsCommand
                    ?? (_viewLogsCommand = new RelayCommand(
                    () =>
                    {
                        //打开日志所在的目录
                        Common.LocateDirInExplorer(_robotPathConfigService.LogsDir);
                    }));
            }
        }


        private RelayCommand _aboutProductCommand;

        /// <summary>
        /// 关于产品窗口
        /// </summary>
        public RelayCommand AboutProductCommand
        {
            get
            {
                return _aboutProductCommand
                    ?? (_aboutProductCommand = new RelayCommand(
                    () =>
                    {
                        if (!_startupViewModel.AboutWindow.IsVisible)
                        {
                            _startupViewModel.AboutWindow.Show();
                        }

                        _startupViewModel.AboutWindow.Activate();
                    }));
            }
        }





        /// <summary>
        /// The <see cref="PackageItems" /> property's name.
        /// </summary>
        public const string PackageItemsPropertyName = "PackageItems";

        private ObservableCollection<PackageItemViewModel> _packageItemsProperty = new ObservableCollection<PackageItemViewModel>();

        /// <summary>
        /// 包列表条目
        /// </summary>
        public ObservableCollection<PackageItemViewModel> PackageItems
        {
            get
            {
                return _packageItemsProperty;
            }

            set
            {
                if (_packageItemsProperty == value)
                {
                    return;
                }

                _packageItemsProperty = value;
                RaisePropertyChanged(PackageItemsPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsSearchResultEmpty" /> property's name.
        /// </summary>
        public const string IsSearchResultEmptyPropertyName = "IsSearchResultEmpty";

        private bool _isSearchResultEmptyProperty = false;

        /// <summary>
        /// 搜索结果是否为空
        /// </summary>
        public bool IsSearchResultEmpty
        {
            get
            {
                return _isSearchResultEmptyProperty;
            }

            set
            {
                if (_isSearchResultEmptyProperty == value)
                {
                    return;
                }

                _isSearchResultEmptyProperty = value;
                RaisePropertyChanged(IsSearchResultEmptyPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="SearchText" /> property's name.
        /// </summary>
        public const string SearchTextPropertyName = "SearchText";

        private string _searchTextProperty = "";

        /// <summary>
        /// 搜索文本
        /// </summary>
        public string SearchText
        {
            get
            {
                return _searchTextProperty;
            }

            set
            {
                if (_searchTextProperty == value)
                {
                    return;
                }

                _searchTextProperty = value;
                RaisePropertyChanged(SearchTextPropertyName);

                doSearch();
            }
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        private void doSearch()
        {
            var searchContent = SearchText.Trim();
            if (string.IsNullOrEmpty(searchContent))
            {
                //还原起始显示
                foreach (var item in PackageItems)
                {
                    item.IsSearching = false;
                }

                foreach (var item in PackageItems)
                {
                    item.SearchText = searchContent;
                }

                IsSearchResultEmpty = false;
            }
            else
            {
                //根据搜索内容显示

                foreach (var item in PackageItems)
                {
                    item.IsSearching = true;
                }

                //预先全部置为不匹配
                foreach (var item in PackageItems)
                {
                    item.IsMatch = false;
                }


                foreach (var item in PackageItems)
                {
                    item.ApplyCriteria(searchContent);
                }

                IsSearchResultEmpty = true;
                foreach (var item in PackageItems)
                {
                    if (item.IsMatch)
                    {
                        IsSearchResultEmpty = false;
                        break;
                    }
                }

            }
        }




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The <see cref="IsWorkflowRunning" /> property's name.
        /// </summary>
        public const string IsWorkflowRunningPropertyName = "IsWorkflowRunning";

        private bool _isWorkflowRunningProperty = false;

        /// <summary>
        /// 工作流是否正在运行
        /// </summary>
        public bool IsWorkflowRunning
        {
            get
            {
                return _isWorkflowRunningProperty;
            }

            set
            {
                if (_isWorkflowRunningProperty == value)
                {
                    return;
                }

                _isWorkflowRunningProperty = value;
                RaisePropertyChanged(IsWorkflowRunningPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="WorkflowRunningToolTip" /> property's name.
        /// </summary>
        public const string WorkflowRunningToolTipPropertyName = "WorkflowRunningToolTip";

        private string _workflowRunningToolTipProperty = "";

        /// <summary>
        /// 工作流运行提示信息
        /// </summary>
        public string WorkflowRunningToolTip
        {
            get
            {
                return _workflowRunningToolTipProperty;
            }

            set
            {
                if (_workflowRunningToolTipProperty == value)
                {
                    return;
                }

                _workflowRunningToolTipProperty = value;
                RaisePropertyChanged(WorkflowRunningToolTipPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="WorkflowRunningName" /> property's name.
        /// </summary>
        public const string WorkflowRunningNamePropertyName = "WorkflowRunningName";

        private string _workflowRunningNameProperty = "";

        /// <summary>
        /// 工作流运行名称
        /// </summary>
        public string WorkflowRunningName
        {
            get
            {
                return _workflowRunningNameProperty;
            }

            set
            {
                if (_workflowRunningNameProperty == value)
                {
                    return;
                }

                _workflowRunningNameProperty = value;
                RaisePropertyChanged(WorkflowRunningNamePropertyName);
            }
        }





        /// <summary>
        /// The <see cref="WorkflowRunningStatus" /> property's name.
        /// </summary>
        public const string WorkflowRunningStatusPropertyName = "WorkflowRunningStatus";

        private string _workflowRunningStatusProperty = "";

        /// <summary>
        /// 工作流运行状态 
        /// </summary>
        public string WorkflowRunningStatus
        {
            get
            {
                return _workflowRunningStatusProperty;
            }

            set
            {
                if (_workflowRunningStatusProperty == value)
                {
                    return;
                }

                _workflowRunningStatusProperty = value;
                RaisePropertyChanged(WorkflowRunningStatusPropertyName);
            }
        }




        private RelayCommand _stopCommand;

        /// <summary>
        /// 停止运行
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand
                    ?? (_stopCommand = new RelayCommand(
                    () =>
                    {
                        if (_runManagerService != null)
                        {
                            _runManagerService.Stop();
                        }
                    },
                    () => true));
            }
        }


    }
}