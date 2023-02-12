using Activities.Shared.ActivityTemplateFactory;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using Newtonsoft.Json.Linq;
using RPA.Interfaces.Activities;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Extensions;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProjectViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;
        private IProjectManagerService _projectManagerService;
        private IActivitiesServiceProxy _activitiesServiceProxy;

        public IDragSource ProjectItemDragHandler { get; set; }

        public IDropTarget ProjectItemDropHandler { get; set; }

        private ProjectRootItemViewModel _projectRootVM;
        private ProjectDirItemViewModel _projectScreenshotsItemVM;

        /// <summary>
        /// 是否展开字典
        /// </summary>
        public Dictionary<string, bool> IsExpandedDict = new Dictionary<string, bool>();


        private Dictionary<string, ProjectBaseItemViewModel> ItemPathDict = new Dictionary<string, ProjectBaseItemViewModel>();

        /// <summary>
        /// Initializes a new instance of the ProjectViewModel class.
        /// </summary>
        public ProjectViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();
            ProjectItemDragHandler = _serviceLocator.ResolveType<IDragSource>();
            ProjectItemDropHandler = _serviceLocator.ResolveType<IDropTarget>();

            _projectManagerService.ProjectOpenEvent += _projectManagerService_ProjectOpenEvent;
            _projectManagerService.ProjectCloseEvent += _projectManagerService_ProjectCloseEvent;


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


        private void Init(bool bOpenMainXaml = true)
        {
            ProjectItems.Clear();
            ItemPathDict.Clear();
            var fileOrDirItems = Common.QueryDirectoryAndFiles(_projectManagerService.CurrentProjectPath
                , (DirOrFileItem item) => (
                !(
                item is DirItem && item.Name == ProjectConstantConfig.ProjectLocalDirectoryName //过滤掉.local目录
                || (item is FileItem && !new List<string>() { ProjectConstantConfig.XamlFileExtension, ".py", ".js" }.Contains((item as FileItem).Extension.ToLower()))) //过滤掉.xaml,.py及.js以外的文件
                ));

            var projectRoot = _serviceLocator.ResolveType<ProjectRootItemViewModel>();
            _projectRootVM = projectRoot;
            projectRoot.IsExpanded = true;
            projectRoot.Name = _projectManagerService.CurrentProjectJsonConfig.name;
            projectRoot.ProjectPath = _projectManagerService.CurrentProjectPath;
            projectRoot.Path = _projectManagerService.CurrentProjectPath;
            projectRoot.ToolTip = _projectManagerService.CurrentProjectPath;

            //添加依赖包展示
            var dependRootItem = InitDependenciesItems();
            projectRoot.Children.Add(dependRootItem);

            ProjectFileItemViewModel mainXamlFileItem;
            TransformChildren(fileOrDirItems, projectRoot.Children, out mainXamlFileItem);

            ProjectItems.Add(projectRoot);

            if (bOpenMainXaml)
            {
                mainXamlFileItem?.OpenXamlCommand.Execute(null);
            }

            //项目打开后设置当前路径为项目路径
            Directory.SetCurrentDirectory(SharedObject.Instance.ProjectPath);
        }

        private ProjectDependRootItemViewModel InitDependenciesItems()
        {
            var dependRootItem = _serviceLocator.ResolveType<ProjectDependRootItemViewModel>();
            dependRootItem.Name = "依赖包";

            dependRootItem.IsExpanded = true;

            var json_cfg = _projectManagerService.CurrentProjectJsonConfig;

            foreach (JProperty jp in (JToken)json_cfg.dependencies)
            {
                string ver_desc = "";

                //目前只考虑等于的情况
                string minVersion = null;
                if (CommonNuget.VersionRangeIsMinInclusive((string)jp.Value, out minVersion))
                {
                    ver_desc = $" = {minVersion}";
                }

                var desc = jp.Name + ver_desc;

                var dependItem = _serviceLocator.ResolveType<ProjectDependItemViewModel>();
                dependItem.Name = desc;
                dependRootItem.Children.Add(dependItem);
            }

            return dependRootItem;
        }

        private void _projectManagerService_ProjectOpenEvent(object sender, System.EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsProjectOpened = true;

                var service = sender as IProjectManagerService;

                _activitiesServiceProxy = service.CurrentActivitiesServiceProxy;

                Init();
            });
        }

        private void _projectManagerService_ProjectCloseEvent(object sender, System.EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                IsProjectOpened = false;
                IsSearchResultEmpty = false;

                ProjectItems.Clear();
                IsExpandedDict.Clear();
                ItemPathDict.Clear();
                SearchText = "";
            });
        }

        public void Refresh()
        {
            Init(false);
            StartSearch();
        }

        private void TransformChildren(List<DirOrFileItem> children1, ObservableCollection<ProjectBaseItemViewModel> children2, out ProjectFileItemViewModel mainXamlFileItem)
        {
            mainXamlFileItem = null;

            foreach (var dirOrFileItem in children1)
            {
                if (dirOrFileItem is DirItem)
                {
                    var dirItem = dirOrFileItem as DirItem;
                    var item = _serviceLocator.ResolveType<ProjectDirItemViewModel>();
                    item.Name = dirItem.Name;
                    item.Path = dirItem.Path;
                    item.IsScreenshots = dirItem.Name.ToLower() == ProjectConstantConfig.ScreenshotsPath.ToLower();
                    if (item.IsScreenshots)
                    {
                        _projectScreenshotsItemVM = item;
                    }
                    TransformChildren(dirItem.Children, item.Children, out mainXamlFileItem);
                    children2.Add(item);
                    ItemPathDict[item.Path] = item;
                }
                else if (dirOrFileItem is FileItem)
                {
                    var fileItem = dirOrFileItem as FileItem;
                    var item = _serviceLocator.ResolveType<ProjectFileItemViewModel>();
                    item.Name = fileItem.Name;
                    item.Path = fileItem.Path;

                    if (fileItem.Extension.ToLower() == ProjectConstantConfig.XamlFileExtension.ToLower())
                    {
                        item.Icon = new BitmapImage(new Uri("pack://application:,,,/RPA.Resources;Component/Image/Project/xaml.png"));
                        item.IsXamlFile = true;

                        item.ActivityFactoryAssemblyQualifiedName = _activitiesServiceProxy.GetAssemblyQualifiedName(InvokeWorkflowFileFactory.AssemblyQualifiedName);

                        if (_projectManagerService.ActivitiesTypeOfDict.ContainsKey(InvokeWorkflowFileFactory.ActivityTypeName))
                        {
                            item.ActivityDisplayName = _projectManagerService.ActivitiesTypeOfDict[InvokeWorkflowFileFactory.ActivityTypeName].Name;
                            item.ActivityAssemblyQualifiedName = _activitiesServiceProxy.GetAssemblyQualifiedName(InvokeWorkflowFileFactory.ActivityTypeName);
                        }
                    }
                    else if (fileItem.Name.ToLower() == ProjectConstantConfig.ProjectConfigFileName.ToLower())
                    {
                        item.IsProjectJsonFile = true;
                    }
                    else
                    {
                        if (fileItem.Extension.ToLower() == ".py")
                        {
                            item.IsPythonFile = true;

                            item.ActivityFactoryAssemblyQualifiedName = _activitiesServiceProxy.GetAssemblyQualifiedName(InvokePythonFileFactory.AssemblyQualifiedName);

                            if (_projectManagerService.ActivitiesTypeOfDict.ContainsKey(InvokePythonFileFactory.ActivityTypeName))
                            {
                                item.ActivityDisplayName = _projectManagerService.ActivitiesTypeOfDict[InvokePythonFileFactory.ActivityTypeName].Name;
                                item.ActivityAssemblyQualifiedName = _activitiesServiceProxy.GetAssemblyQualifiedName(InvokePythonFileFactory.ActivityTypeName);
                            }
                        }

                        item.Icon = fileItem.AssociatedIcon;
                    }

                    if (item.Path == _projectManagerService.CurrentProjectMainXamlFileAbsolutePath)
                    {
                        item.IsMainXamlFile = true;

                        mainXamlFileItem = item;
                    }

                    children2.Add(item);
                    ItemPathDict[item.Path] = item;
                }
            }
        }


        public void OnMoveDir(string srcPath, string dstPath)
        {
            Refresh();
        }

        public void OnProjectSettingsModify(ProjectSettingsViewModel projectSettingsViewModel)
        {
            Refresh();
        }


        public void OnDeleteDir(string path)
        {
            Refresh();
        }

        public void OnDeleteFile(string path)
        {
            Refresh();
        }


        public void OnRename(RenameViewModel renameViewModel)
        {
            if (renameViewModel.IsMain)
            {
                //主文件重命名，则需要在project.rpa进行修正
                var relativeMainXaml = Common.MakeRelativePath(_projectManagerService.CurrentProjectPath, renameViewModel.Dir + @"\" + renameViewModel.DstName);
                _projectManagerService.CurrentProjectJsonConfig.main = relativeMainXaml;
                _projectManagerService.SaveCurrentProjectJson();
            }

            var tempIsExpandedDict = new Dictionary<string, bool>();
            foreach (var item in IsExpandedDict)
            {
                if (item.Key.ContainsIgnoreCase(renameViewModel.Path))
                {
                    var newValue = item.Value;

                    var newKey = "";
                    if (item.Key.EqualsIgnoreCase(renameViewModel.Path))
                    {
                        newKey = item.Key.Replace(renameViewModel.Path, renameViewModel.NewPath);
                    }
                    else
                    {
                        newKey = item.Key.Replace(renameViewModel.Path + @"\", renameViewModel.NewPath + @"\");
                    }

                    tempIsExpandedDict[newKey] = newValue;
                }
                else
                {
                    tempIsExpandedDict[item.Key] = item.Value;
                }
            }

            IsExpandedDict = tempIsExpandedDict;

            Refresh();
        }




        /// <summary>
        /// The <see cref="ProjectItems" /> property's name.
        /// </summary>
        public const string ProjectItemsPropertyName = "ProjectItems";

        private ObservableCollection<ProjectRootItemViewModel> _projectItemsProperty = new ObservableCollection<ProjectRootItemViewModel>();

        /// <summary>
        /// 项目节点集合
        /// </summary>
        public ObservableCollection<ProjectRootItemViewModel> ProjectItems
        {
            get
            {
                return _projectItemsProperty;
            }

            set
            {
                if (_projectItemsProperty == value)
                {
                    return;
                }

                _projectItemsProperty = value;
                RaisePropertyChanged(ProjectItemsPropertyName);
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
                        _projectRootVM.OpenDirCommand.Execute(null);
                    }));
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
                        _projectRootVM.OpenProjectSettingsCommand.Execute(null);
                    }));
            }
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
                        //清理前项目文件全部保存一下
                        _serviceLocator.ResolveType<MainViewModel>().SaveAllCommand.Execute(null);
                        //刷新项目树视图
                        RefreshCommand.Execute(null);

                        //清理
                        if (_projectScreenshotsItemVM == null)
                        {
                            CommonMessageBox.ShowInformation("该项目不存在截图目录，无需清理");
                        }
                        else
                        {
                            _projectScreenshotsItemVM.RemoveUnusedScreenshotsCommand.Execute(null);
                        }

                    }));
            }
        }


        /// <summary>
        /// 设置指定节点及其所有子节点的搜索中状态
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="IsSearching">是否正在搜索</param>
        private void ProjectTreeItemSetAllIsSearching(ProjectBaseItemViewModel item, bool IsSearching)
        {
            item.IsSearching = IsSearching;
            foreach (var child in item.Children)
            {
                ProjectTreeItemSetAllIsSearching(child, IsSearching);
            }
        }

        /// <summary>
        /// 设置指定节点及所有子节点的匹配状态
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="IsMatch">是否匹配</param>
        private void ProjectTreeItemSetAllIsMatch(ProjectBaseItemViewModel item, bool IsMatch)
        {
            item.IsMatch = IsMatch;
            foreach (var child in item.Children)
            {
                ProjectTreeItemSetAllIsMatch(child, IsMatch);
            }
        }

        /// <summary>
        /// 设置指定节点及其子节点的搜索文本
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="SearchText">搜索文本</param>
        private void ProjectTreeItemSetAllSearchText(ProjectBaseItemViewModel item, string SearchText)
        {
            item.SearchText = SearchText;
            foreach (var child in item.Children)
            {
                ProjectTreeItemSetAllSearchText(child, SearchText);
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
        /// 实际搜索
        /// </summary>
        private async Task doSearch()
        {
            await Task.Run(() =>
            {
                string searchContent = SearchText ?? "";

                searchContent = searchContent.Trim();

                if (string.IsNullOrEmpty(searchContent))
                {
                    //还原起始显示
                    foreach (var item in ProjectItems)
                    {
                        ProjectTreeItemSetAllIsSearching(item, false);
                    }

                    foreach (var item in ProjectItems)
                    {
                        ProjectTreeItemSetAllSearchText(item, "");
                    }

                    IsSearchResultEmpty = false;
                }
                else
                {
                    //根据搜索内容显示

                    foreach (var item in ProjectItems)
                    {
                        ProjectTreeItemSetAllIsSearching(item, true);
                    }

                    //预先全部置为不匹配
                    foreach (var item in ProjectItems)
                    {
                        ProjectTreeItemSetAllIsMatch(item, false);
                    }


                    foreach (var item in ProjectItems)
                    {
                        item.ApplyCriteria(searchContent, new Stack<ProjectBaseItemViewModel>());
                    }

                    IsSearchResultEmpty = true;
                    foreach (var item in ProjectItems)
                    {
                        if (item.IsMatch)
                        {
                            IsSearchResultEmpty = false;
                            break;
                        }
                    }

                }
            });
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

                StartSearch();

            }
        }

        public ProjectBaseItemViewModel GetProjectItemByFullPath(string xamlFilePath)
        {
            if (ItemPathDict.ContainsKey(xamlFilePath))
            {
                return ItemPathDict[xamlFilePath];
            }

            return null;
        }

        private async void StartSearch()
        {
            await doSearch();
        }



        private RelayCommand _refreshCommand;

        /// <summary>
        /// Gets the RefreshCommand.
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                    ?? (_refreshCommand = new RelayCommand(
                    () =>
                    {
                        Refresh();
                    }));
            }
        }


        private void ProjectTreeItemSetAllIsExpanded(ProjectBaseItemViewModel item, bool IsExpanded)
        {
            item.IsExpanded = IsExpanded;
            foreach (var child in item.Children)
            {
                ProjectTreeItemSetAllIsExpanded(child, IsExpanded);
            }
        }

        private RelayCommand _expandAllCommand;

        /// <summary>
        /// Gets the ExpandAllCommand.
        /// </summary>
        public RelayCommand ExpandAllCommand
        {
            get
            {
                return _expandAllCommand
                    ?? (_expandAllCommand = new RelayCommand(
                    () =>
                    {
                        foreach (var item in ProjectItems)
                        {
                            ProjectTreeItemSetAllIsExpanded(item, true);
                        }
                    }));
            }
        }



        private RelayCommand _collapseAllCommand;

        /// <summary>
        /// Gets the CollapseAllCommand.
        /// </summary>
        public RelayCommand CollapseAllCommand
        {
            get
            {
                return _collapseAllCommand
                    ?? (_collapseAllCommand = new RelayCommand(
                    () =>
                    {
                        foreach (var item in ProjectItems)
                        {
                            ProjectTreeItemSetAllIsExpanded(item, false);
                        }
                    }));
            }
        }


    }
}