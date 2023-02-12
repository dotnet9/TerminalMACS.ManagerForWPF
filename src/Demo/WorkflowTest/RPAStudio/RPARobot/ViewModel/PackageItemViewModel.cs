using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json.Linq;
using NLog;
using NuGet;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Extensions;
using RPA.Shared.Utils;
using RPARobot.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RPARobot.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PackageItemViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 包
        /// </summary>
        public IPackage Package { get; internal set; }

        /// <summary>
        /// 版本列表
        /// </summary>
        public List<string> _versionList { get; set; } = new List<string>();

        private IServiceLocator _serviceLocator;

        private MainViewModel _mainViewModel;

        private StartupViewModel _startupViewModel;

        private IRunManagerService _runManagerService;

        public PackageItemViewModel(IServiceLocator serviceLocator,MainViewModel mainViewModel, StartupViewModel startupViewModel, IRunManagerService runManagerService)
        {
            _serviceLocator = serviceLocator;

            _mainViewModel = mainViewModel;

            _startupViewModel = startupViewModel;

            _runManagerService = runManagerService;
        }

        /// <summary>
        /// The <see cref="IsRunning" /> property's name.
        /// </summary>
        public const string IsRunningPropertyName = "IsRunning";

        private bool _isRunningProperty = false;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _isRunningProperty;
            }

            set
            {
                if (_isRunningProperty == value)
                {
                    return;
                }

                _isRunningProperty = value;
                RaisePropertyChanged(IsRunningPropertyName);

                //更新按钮状态
                StartCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _nameProperty = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _nameProperty;
            }

            set
            {
                if (_nameProperty == value)
                {
                    return;
                }

                _nameProperty = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Version" /> property's name.
        /// </summary>
        public const string VersionPropertyName = "Version";

        private string _versionProperty = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get
            {
                return _versionProperty;
            }

            set
            {
                if (_versionProperty == value)
                {
                    return;
                }

                _versionProperty = value;
                RaisePropertyChanged(VersionPropertyName);
            }
        }


        
        /// <summary>
        /// The <see cref="IsMouseOver" /> property's name.
        /// </summary>
        public const string IsMouseOverPropertyName = "IsMouseOver";

        private bool _isMouseOverProperty = false;

        /// <summary>
        /// 鼠标是否悬浮
        /// </summary>
        public bool IsMouseOver
        {
            get
            {
                return _isMouseOverProperty;
            }

            set
            {
                if (_isMouseOverProperty == value)
                {
                    return;
                }

                _isMouseOverProperty = value;
                RaisePropertyChanged(IsMouseOverPropertyName);
            }
        }



        private RelayCommand _mouseEnterCommand;

        /// <summary>
        /// 鼠标进入
        /// </summary>
        public RelayCommand MouseEnterCommand
        {
            get
            {
                return _mouseEnterCommand
                    ?? (_mouseEnterCommand = new RelayCommand(
                    () =>
                    {
                        IsMouseOver = true;
                    }));
            }
        }


        private RelayCommand _mouseLeaveCommand;

        /// <summary>
        /// 鼠标离开
        /// </summary>
        public RelayCommand MouseLeaveCommand
        {
            get
            {
                return _mouseLeaveCommand
                    ?? (_mouseLeaveCommand = new RelayCommand(
                    () =>
                    {
                        IsMouseOver = false;
                    }));
            }
        }


        /// <summary>
        /// The <see cref="IsNeedUpdate" /> property's name.
        /// </summary>
        public const string IsNeedUpdatePropertyName = "IsNeedUpdate";

        private bool _isNeedUpdateProperty = false;

        /// <summary>
        /// 是否需要更新
        /// </summary>
        public bool IsNeedUpdate
        {
            get
            {
                return _isNeedUpdateProperty;
            }

            set
            {
                if (_isNeedUpdateProperty == value)
                {
                    return;
                }

                _isNeedUpdateProperty = value;
                RaisePropertyChanged(IsNeedUpdatePropertyName);
            }
        }


        private RelayCommand _copyItemInfoCommand;

        /// <summary>
        /// 复制条目信息
        /// </summary>
        public RelayCommand CopyItemInfoCommand
        {
            get
            {
                return _copyItemInfoCommand
                    ?? (_copyItemInfoCommand = new RelayCommand(
                    () =>
                    {
                        Clipboard.SetDataObject(ToolTip);
                    }));
            }
        }


        private RelayCommand _locateItemCommand;

        /// <summary>
        /// 定位包条目所在位置
        /// </summary>
        public RelayCommand LocateItemCommand
        {
            get
            {
                return _locateItemCommand
                    ?? (_locateItemCommand = new RelayCommand(
                    () =>
                    {
                        var file = _mainViewModel.ProgramDataPackagesDir + @"\" + Name + @"." + Version + ".nupkg";
                        Common.LocateFileInExplorer(file);
                    }));
            }
        }


        /// <summary>
        /// 删除nupkg安装包
        /// </summary>
        /// <param name="bRefresh">是否需要刷新包列表</param>
        void DeleteNuPkgsFile(bool bRefresh = true)
        {
            //删除nupkg安装包
            foreach (var ver in _versionList)
            {
                var file = _mainViewModel.ProgramDataPackagesDir + @"\" + Name + @"." + ver + ".nupkg";
                Common.DeleteFile(file);
            }

            if (bRefresh)
            {
                Common.InvokeOnUI(() =>
                {
                    //刷新
                    _mainViewModel.RefreshCommand.Execute(null);
                });
            }

        }


        private RelayCommand _removeItemCommand;

        /// <summary>
        /// 移除包条目
        /// </summary>
        public RelayCommand RemoveItemCommand
        {
            get
            {
                return _removeItemCommand
                    ?? (_removeItemCommand = new RelayCommand(
                    () =>
                    {
                        //确定移除当前包吗？ 询问
                        var ret = CommonMessageBox.ShowQuestion("确认移除吗？");
                        if (ret)
                        {
                            //卸载已安装的包，删除nupkg包
                            if (this.Package != null)
                            {
                                var repo = PackageRepositoryFactory.Default.CreateRepository(_mainViewModel.ProgramDataPackagesDir);
                                var packageManager = new PackageManager(repo, _mainViewModel.ProgramDataInstalledPackagesDir);

                                packageManager.PackageUninstalled += (sender, eventArgs) =>
                                {
                                    //如果都卸载完了，才刷新
                                    if (!packageManager.LocalRepository.Exists(this.Name))
                                    {
                                        DeleteNuPkgsFile();
                                    }
                                };

                                if (packageManager.LocalRepository.Exists(this.Name))
                                {
                                    while (packageManager.LocalRepository.Exists(this.Name))
                                    {
                                        packageManager.UninstallPackage(this.Name);
                                    }

                                }
                                else
                                {
                                    DeleteNuPkgsFile();
                                }


                            }
                        }
                    },
                    () => !IsRunning));
            }
        }

        private RelayCommand _mouseRightButtonUpCommand;

        /// <summary>
        /// 鼠标右键松开事件
        /// </summary>
        public RelayCommand MouseRightButtonUpCommand
        {
            get
            {
                return _mouseRightButtonUpCommand
                    ?? (_mouseRightButtonUpCommand = new RelayCommand(
                    () =>
                    {
                        var view = App.Current.MainWindow;
                        var cm = view.FindResource("PackageItemContextMenu") as ContextMenu;
                        cm.DataContext = this;
                        cm.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                        cm.IsOpen = true;
                    }));
            }
        }




        private RelayCommand _mouseDoubleClickCommand;

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public RelayCommand MouseDoubleClickCommand
        {
            get
            {
                return _mouseDoubleClickCommand
                    ?? (_mouseDoubleClickCommand = new RelayCommand(
                    () =>
                    {
                        updateOrStart();
                    }));
            }
        }

        /// <summary>
        /// 如果未安装，则安装，如果需要更新则更新，如果能运行则运行，只执行一个步骤
        /// </summary>
        private void updateOrStart()
        {
            if (IsNeedUpdate)
            {
                UpdateCommand.Execute(null);
            }
            else
            {
                StartCommand.Execute(null);
            }
        }



        /// <summary>
        /// The <see cref="IsVisible" /> property's name.
        /// </summary>
        public const string IsVisiblePropertyName = "IsVisible";

        private bool _isVisibleProperty = true;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return _isVisibleProperty;
            }

            set
            {
                if (_isVisibleProperty == value)
                {
                    return;
                }

                _isVisibleProperty = value;
                RaisePropertyChanged(IsVisiblePropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        private bool _isSelectedProperty = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelectedProperty;
            }

            set
            {
                if (_isSelectedProperty == value)
                {
                    return;
                }

                _isSelectedProperty = value;
                RaisePropertyChanged(IsSelectedPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="ToolTip" /> property's name.
        /// </summary>
        public const string ToolTipPropertyName = "ToolTip";

        private string _toolTipProperty = null;

        /// <summary>
        /// 提示信息
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




        /// <summary>
        /// The <see cref="IsSearching" /> property's name.
        /// </summary>
        public const string IsSearchingPropertyName = "IsSearching";

        private bool _isSearchingProperty = false;

        /// <summary>
        /// 是否处在搜索中 
        /// </summary>
        public bool IsSearching
        {
            get
            {
                return _isSearchingProperty;
            }

            set
            {
                if (_isSearchingProperty == value)
                {
                    return;
                }

                _isSearchingProperty = value;
                RaisePropertyChanged(IsSearchingPropertyName);
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
            }
        }



        /// <summary>
        /// The <see cref="IsMatch" /> property's name.
        /// </summary>
        public const string IsMatchPropertyName = "IsMatch";

        private bool _isMatchProperty = false;

        /// <summary>
        /// 是否匹配
        /// </summary>
        public bool IsMatch
        {
            get
            {
                return _isMatchProperty;
            }

            set
            {
                if (_isMatchProperty == value)
                {
                    return;
                }

                _isMatchProperty = value;
                RaisePropertyChanged(IsMatchPropertyName);
            }
        }


        /// <summary>
        /// 应用关键词
        /// </summary>
        /// <param name="criteria">关键词</param>
        public void ApplyCriteria(string criteria)
        {
            SearchText = criteria;

            if (IsCriteriaMatched(criteria))
            {
                IsMatch = true;

            }
        }

        /// <summary>
        /// 关键词是否匹配
        /// </summary>
        /// <param name="criteria">关键词</param>
        /// <returns>是否匹配</returns>
        private bool IsCriteriaMatched(string criteria)
        {
            return string.IsNullOrEmpty(criteria) || Name.ContainsIgnoreCase(criteria);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        private RelayCommand _startCommand;

        /// <summary>
        /// 启动流程
        /// </summary>
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand
                    ?? (_startCommand = new RelayCommand(
                    () =>
                    {
                        //如果已经有一个项目正在运行，则不允许再运行
                        if (_mainViewModel.IsWorkflowRunning)
                        {
                            var msg = "已经有工作流正在运行，请等待它结束后再运行！";
                            CommonMessageBox.ShowWarning(msg);
                            return;
                        }

                        var projectDir = _mainViewModel.ProgramDataInstalledPackagesDir + @"\" + Name + @"." + Version + @"\lib\net45";
                        var projectJsonFile = projectDir + @"\"+ ProjectConstantConfig.ProjectConfigFileNameWithSuffix; 
                        if (System.IO.File.Exists(projectJsonFile))
                        {
                            //项目配置文件存在
                            Task.Run(async () =>
                            {
                                try
                                {
                                    //加载项目依赖项
                                    Common.InvokeOnUI(() => {
                                        //隐藏运行按钮，其实这里并未实际执行，只是在加载依赖项
                                        this.IsRunning = true;
                                        _mainViewModel.IsWorkflowRunning = true;
                                        _mainViewModel.WorkflowRunningName = Name;
                                        _mainViewModel.WorkflowRunningToolTip = ToolTip;
                                        _mainViewModel.WorkflowRunningStatus = "正在加载项目依赖项"; //正在加载项目依赖项
                                    });
                                    var serv = _serviceLocator.ResolveType<ILoadDependenciesService>();
                                    serv.Init(projectJsonFile);
                                    await serv.LoadDependencies();

                                    //1.找到主XAML文件，然后运行它
                                    string json = System.IO.File.ReadAllText(projectJsonFile);
                                    JObject jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
                                    var relativeMainXaml = jsonObj["main"].ToString();
                                    var absoluteMainXaml = System.IO.Path.Combine(projectDir, relativeMainXaml);

                                    if (System.IO.File.Exists(absoluteMainXaml))
                                    {
                                        RunWorkflow(projectDir, absoluteMainXaml, serv.CurrentActivitiesDllLoadFrom, serv.CurrentDependentAssemblies);
                                    }
                                    else
                                    {
                                        Common.InvokeOnUI(() => {
                                            CommonMessageBox.ShowError("找不到主流程文件来运行！");
                                        });
                                    }
                                }
                                catch (Exception err)
                                {
                                    _logger.Error(err);

                                    Common.InvokeOnUI(() => {
                                        this.IsRunning = false;
                                        var msg = "加载项目依赖项出错！";
                                        CommonMessageBox.ShowError(msg);
                                    });
                                }
                            });
                        }
                        else
                        {
                            var msg = "找不到项目配置文件："+ projectJsonFile;

                            CommonMessageBox.ShowError(msg);
                        }

                    },
                    () => !IsRunning));
            }
        }


        /// <summary>
        /// 运行工作流
        /// </summary>
        /// <param name="projectDir">项目目录</param>
        /// <param name="absoluteMainXaml">main.xaml绝对路径</param>
        private void RunWorkflow(string projectDir, string absoluteMainXaml, List<string> activitiesDllLoadFrom, List<string> dependentAssemblies)
        {
            System.GC.Collect();//提醒系统回收内存，避免内存占用过高

            SharedObject.Instance.ProjectPath = projectDir;
            SharedObject.Instance.OutputEvent -= Instance_OutputEvent;
            SharedObject.Instance.OutputEvent += Instance_OutputEvent;

            _runManagerService.Init(this, absoluteMainXaml, activitiesDllLoadFrom, dependentAssemblies);
            _runManagerService.Run();
        }

        private void Instance_OutputEvent(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            _runManagerService.LogToOutputWindow(type, msg, msgDetails);
        }

        private RelayCommand _updateCommand;

        /// <summary>
        /// 更新包
        /// </summary>
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand
                    ?? (_updateCommand = new RelayCommand(
                    () =>
                    {
                        //安装或更新包
                        if (this.Package != null)
                        {
                            var repo = PackageRepositoryFactory.Default.CreateRepository(_mainViewModel.ProgramDataPackagesDir);
                            var packageManager = new PackageManager(repo, _mainViewModel.ProgramDataInstalledPackagesDir);

                            packageManager.PackageInstalled += (sender, eventArgs) =>
                            {
                                _mainViewModel.RefreshCommand.Execute(null);
                            };

                            packageManager.InstallPackage(this.Package, true, true, true);
                        }
                    },
                    () => !IsRunning));
            }
        }
    }
}