using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using RPA.Interfaces.Activities;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Shared.Configs;
using RPA.Shared.Extensions;
using RPA.Shared.Utils;
using RPAStudio.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ActivitiesViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IServiceLocator _serviceLocator;

        private IProjectManagerService _projectManagerService;

        private IActivityFavoritesService _activityFavoritesService;
        private IActivityRecentService _activityRecentService;
        private IActivityMountService _activityMountService;
        private ISystemActivityIconService _systemActivityIconService;

        private IActivitiesServiceProxy _activitiesServiceProxy;//非单例

        public ActivityItemDragHandler ActivityItemDragHandler { get; set; } = new ActivityItemDragHandler();

        private Dictionary<string, ActivityLeafItemViewModel> ActivityLeafItemTypeOfDict = new Dictionary<string, ActivityLeafItemViewModel>();
        private Dictionary<string, ActivityLeafItemViewModel> AssemblyQualifiedNameDict = new Dictionary<string, ActivityLeafItemViewModel>();

        public ActivityGroupItemViewModel FavoritesGroupItem { get; private set; }
        public ActivityGroupItemViewModel RecentGroupItem { get; private set; }
        public ActivityGroupItemViewModel TemplateGroupItem { get; private set; }
        public ActivityGroupItemViewModel ActivitiesGroupItem { get; private set; }


        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the ActivitiesViewModel class.
        /// </summary>
        public ActivitiesViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();

            _activityFavoritesService = _serviceLocator.ResolveType<IActivityFavoritesService>();
            _activityRecentService = _serviceLocator.ResolveType<IActivityRecentService>();
            _activityMountService = _serviceLocator.ResolveType<IActivityMountService>();
            _systemActivityIconService = _serviceLocator.ResolveType<ISystemActivityIconService>();

            _projectManagerService.ProjectOpenEvent += _projectManagerService_ProjectOpenEvent;
            _projectManagerService.ProjectCloseEvent += _projectManagerService_ProjectCloseEvent;
        }


        private void _projectManagerService_ProjectOpenEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsProjectOpened = true;

                _activitiesServiceProxy = _projectManagerService.CurrentActivitiesServiceProxy;

                //先初始化组件列表，再处理收藏和最近
                var activityGroup = ActivitiesItemsAppendActivity();
                var activities = _projectManagerService.Activities;
                try
                {
                    ActivitiesItemsAppendActivities(activityGroup.Children, activities);
                }
                catch (Exception err)
                {
                    _logger.Error(err);
                }


                //收藏和最近共享组件列表中的条目
                var favoritesList = _activityFavoritesService.Query();
                ActivitiesItemsAppendFavorites(favoritesList);

                var recentList = _activityRecentService.Query();
                ActivitiesItemsAppendRecent(recentList);

                //排序
                ActivitiesItemsSortByGroupType();
                FavoritesGroupItemSort();
            });

        }


        private void FavoritesGroupItemSort()
        {
            //按名字排序
            FavoritesGroupItem.Children.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        private void ActivitiesItemsSortByGroupType()
        {
            ActivityItems.Sort((x, y) => x.GroupType.CompareTo(y.GroupType));
        }


        //根据TypeOf值获取组件的ICON
        public ImageSource GetIconByAssemblyQualifiedName(string assemblyQualifiedName)
        {
            if (AssemblyQualifiedNameDict.ContainsKey(assemblyQualifiedName))
            {
                var item = AssemblyQualifiedNameDict[assemblyQualifiedName];

                return item.IconSource;
            }

            return null;
        }

        private void _projectManagerService_ProjectCloseEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsProjectOpened = false;
                IsSearchResultEmpty = false;
                ActivityItems.Clear();
                SearchText = "";
                ActivityLeafItemTypeOfDict.Clear();
                AssemblyQualifiedNameDict.Clear();
            });
        }


        private void GroupChildrenRemove(ActivityGroupItemViewModel group, string typeOf)
        {
            foreach (var item in group.Children)
            {
                var leafItemVM = item as ActivityLeafItemViewModel;
                if (leafItemVM.TypeOf == typeOf)
                {
                    group.Children.Remove(item);
                    break;
                }
            }
        }

        private void GroupChildrenLimit(ActivityGroupItemViewModel group, int maxCount)
        {
            while (group.Children.Count >= maxCount)
            {
                group.Children.Remove(group.Children.LastOrDefault());
            }
        }

        private void GroupChildrenAddFront(ActivityGroupItemViewModel group, string typeOf)
        {
            if (ActivityLeafItemTypeOfDict.ContainsKey(typeOf))
            {
                group.Children.Insert(0, ActivityLeafItemTypeOfDict[typeOf]);
            }
        }

        /// <summary>
        /// 添加到最近列表
        /// </summary>
        /// <param name="typeOf"></param>
        public void AddToRecent(string typeOf)
        {
            _activityRecentService.Add(typeOf);

            GroupChildrenRemove(RecentGroupItem, typeOf);

            GroupChildrenLimit(RecentGroupItem, ProjectConstantConfig.ActivitiesRecentGroupMaxRecordCount);
            GroupChildrenAddFront(RecentGroupItem, typeOf);
        }

        public void RemoveFromFavorites(ActivityLeafItemViewModel activityLeafItemViewModel)
        {
            activityLeafItemViewModel.IsInFavorites = false;
            _activityFavoritesService.Remove(activityLeafItemViewModel.TypeOf);

            GroupChildrenRemove(FavoritesGroupItem, activityLeafItemViewModel.TypeOf);
        }

        public void AddToFavorites(ActivityLeafItemViewModel activityLeafItemViewModel)
        {
            activityLeafItemViewModel.IsInFavorites = true;
            _activityFavoritesService.Add(activityLeafItemViewModel.TypeOf);

            GroupChildrenRemove(FavoritesGroupItem, activityLeafItemViewModel.TypeOf);
            GroupChildrenAddFront(FavoritesGroupItem, activityLeafItemViewModel.TypeOf);

            FavoritesGroupItemSort();
        }
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="favoritesList"></param>
        private void ActivitiesItemsAppendFavorites(List<ActivityGroupOrLeafItem> favoritesList)
        {
            var groupItem = _serviceLocator.ResolveType<ActivityGroupItemViewModel>();
            ActivityItems.Add(groupItem);
            FavoritesGroupItem = groupItem;

            groupItem.Name = "收藏";
            groupItem.ToolTip = "收藏的内容按组件名称排序";
            groupItem.GroupType = ActivityBaseItemViewModel.enGroupType.Favorites;
            groupItem.Icon = "pack://application:,,,/RPA.Resources;Component/Image/Activities/favorites.png";
            groupItem.IsExpanded = false;

            FavoritesOrRecentGroupAppendItems(groupItem, favoritesList, true);
        }

        /// <summary>
        /// 最近
        /// </summary>
        /// <param name="recentList"></param>
        private void ActivitiesItemsAppendRecent(List<ActivityGroupOrLeafItem> recentList)
        {
            var groupItem = _serviceLocator.ResolveType<ActivityGroupItemViewModel>();
            ActivityItems.Add(groupItem);
            RecentGroupItem = groupItem;

            groupItem.Name = "最近";
            groupItem.ToolTip = "最近的内容按拖动组件的时间排序";
            groupItem.GroupType = ActivityBaseItemViewModel.enGroupType.Recent;
            groupItem.Icon = "pack://application:,,,/RPA.Resources;Component/Image/Activities/recent.png";
            groupItem.IsExpanded = false;

            FavoritesOrRecentGroupAppendItems(groupItem, recentList, false);
        }


        private ActivityGroupItemViewModel ActivitiesItemsAppendActivity()
        {
            var groupItem = _serviceLocator.ResolveType<ActivityGroupItemViewModel>();
            ActivityItems.Add(groupItem);
            ActivitiesGroupItem = groupItem;

            groupItem.Name = "组件列表";
            groupItem.GroupType = ActivityBaseItemViewModel.enGroupType.Activities;
            groupItem.Icon = "pack://application:,,,/RPA.Resources;Component/Image/Activities/activities.png";
            groupItem.IsExpanded = true;

            return groupItem;
        }



        private void FavoritesOrRecentGroupAppendItems(ActivityGroupItemViewModel groupItem, List<ActivityGroupOrLeafItem> items, bool isInFavorites)
        {
            foreach (var favorOrRecentItem in items)
            {
                var leafItem = favorOrRecentItem as ActivityLeafItem;

                if (ActivityLeafItemTypeOfDict.ContainsKey(leafItem.TypeOf))
                {
                    var leafItemVM = ActivityLeafItemTypeOfDict[leafItem.TypeOf];
                    if (isInFavorites)
                    {
                        leafItemVM.IsInFavorites = true;
                    }

                    groupItem.Children.Add(leafItemVM);
                }
            }
        }




        /// <summary>
        /// 活动条目添加，包括系统自带的和用户自定义的所有活动
        /// </summary>
        /// <param name="list"></param>
        private void ActivitiesItemsAppendActivities(ObservableCollection<ActivityBaseItemViewModel> vmList, List<ActivityGroupOrLeafItem> list)
        {
            int initOrder = 0;
            foreach (var groupOrLeafItem in list)
            {
                initOrder++;
                if (groupOrLeafItem is ActivityGroupItem)
                {
                    var groupItem = groupOrLeafItem as ActivityGroupItem;

                    var groupItemVM = _serviceLocator.ResolveType<ActivityGroupItemViewModel>();
                    groupItemVM.InitOrder = initOrder;
                    vmList.Add(groupItemVM);

                    groupItemVM.Name = groupItem.Name;

                    ActivitiesItemsAppendActivities(groupItemVM.Children, groupItem.Children);
                }
                else
                {
                    var leafItem = groupOrLeafItem as ActivityLeafItem;

                    var leafItemVM = _serviceLocator.ResolveType<ActivityLeafItemViewModel>();
                    leafItemVM.InitOrder = initOrder;
                    vmList.Add(leafItemVM);

                    leafItemVM.Name = leafItem.Name;
                    leafItemVM.TypeOf = leafItem.TypeOf;
                    leafItemVM.AssemblyQualifiedName = _activitiesServiceProxy.GetAssemblyQualifiedName(leafItem.TypeOf);
                    leafItemVM.ToolTip = leafItem.ToolTip;


                    ActivityLeafItemTypeOfDict[leafItemVM.TypeOf] = leafItemVM;
                    AssemblyQualifiedNameDict[leafItemVM.AssemblyQualifiedName] = leafItemVM;


                    if (_projectManagerService.ActivitiesTypeOfDict.ContainsKey(leafItemVM.TypeOf))
                    {
                        var item = _projectManagerService.ActivitiesTypeOfDict[leafItemVM.TypeOf] as ActivityLeafItem;

                        if (!string.IsNullOrEmpty(item.Icon))
                        {
                            string[] sArray = item.TypeOf.Split(',');
                            if (sArray.Length > 1)
                            {
                                leafItemVM.IconSource = _activitiesServiceProxy.GetIcon(sArray[1], item.Icon);
                            }
                        }
                        else
                        {
                            leafItemVM.IconSource = _systemActivityIconService.GetIcon(item.TypeOf);
                        }
                    }


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
        /// The <see cref="ActivityItems" /> property's name.
        /// </summary>
        public const string ActivityItemsPropertyName = "ActivityItems";

        private ObservableCollection<ActivityBaseItemViewModel> _activityItemsProperty = new ObservableCollection<ActivityBaseItemViewModel>();

        /// <summary>
        /// Sets and gets the ActivityItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ActivityBaseItemViewModel> ActivityItems
        {
            get
            {
                return _activityItemsProperty;
            }

            set
            {
                if (_activityItemsProperty == value)
                {
                    return;
                }

                _activityItemsProperty = value;
                RaisePropertyChanged(ActivityItemsPropertyName);
            }
        }






        /// <summary>
        /// 设置指定节点及其所有子节点的搜索中状态
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="IsSearching">是否正在搜索</param>
        private void ActivityTreeItemSetAllIsSearching(ActivityBaseItemViewModel item, bool IsSearching)
        {
            item.IsSearching = IsSearching;
            foreach (var child in item.Children)
            {
                ActivityTreeItemSetAllIsSearching(child, IsSearching);
            }
        }

        /// <summary>
        /// 设置指定节点及所有子节点的匹配状态
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="IsMatch">是否匹配</param>
        private void ActivityTreeItemSetAllIsMatch(ActivityBaseItemViewModel item, bool IsMatch)
        {
            item.IsMatch = IsMatch;
            foreach (var child in item.Children)
            {
                ActivityTreeItemSetAllIsMatch(child, IsMatch);
            }
        }

        /// <summary>
        /// 设置指定节点及其子节点的搜索文本
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="SearchText">搜索文本</param>
        private void ActivityTreeItemSetAllSearchText(ActivityBaseItemViewModel item, string SearchText)
        {
            item.SearchText = SearchText;
            foreach (var child in item.Children)
            {
                ActivityTreeItemSetAllSearchText(child, SearchText);
            }
        }

        private void ActivityTreeItemSetAllIsExpanded(ActivityBaseItemViewModel item, bool IsExpanded)
        {
            item.IsExpanded = IsExpanded;
            foreach (var child in item.Children)
            {
                ActivityTreeItemSetAllIsExpanded(child, IsExpanded);
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
                        foreach (var item in ActivityItems)
                        {
                            ActivityTreeItemSetAllIsExpanded(item, true);
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
                        foreach (var item in ActivityItems)
                        {
                            ActivityTreeItemSetAllIsExpanded(item, false);
                        }
                    }));
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
            try
            {
                _tokenSource.Cancel();//取消上次可能正在运行未完成的Task

                _tokenSource = new CancellationTokenSource();
                CancellationToken ct = _tokenSource.Token;

                await Task.Run(() =>
                {
                    ct.ThrowIfCancellationRequested();

                    string searchContent = SearchText ?? "";

                    searchContent = searchContent.Trim();

                    if (string.IsNullOrEmpty(searchContent))
                    {
                        //还原起始显示
                        foreach (var item in ActivityItems)
                        {
                            ct.ThrowIfCancellationRequested();

                            ActivityTreeItemSetAllIsSearching(item, false);
                        }

                        foreach (var item in ActivityItems)
                        {
                            ct.ThrowIfCancellationRequested();

                            ActivityTreeItemSetAllSearchText(item, "");
                        }

                        IsSearchResultEmpty = false;
                    }
                    else
                    {
                        //根据搜索内容显示

                        foreach (var item in ActivityItems)
                        {
                            ct.ThrowIfCancellationRequested();

                            ActivityTreeItemSetAllIsSearching(item, true);
                        }

                        //预先全部置为不匹配
                        foreach (var item in ActivityItems)
                        {
                            ct.ThrowIfCancellationRequested();

                            ActivityTreeItemSetAllIsMatch(item, false);
                        }


                        foreach (var item in ActivityItems)
                        {
                            ct.ThrowIfCancellationRequested();

                            item.ApplyCriteria(searchContent, new Stack<ActivityBaseItemViewModel>());
                        }

                        IsSearchResultEmpty = true;
                        foreach (var item in ActivityItems)
                        {
                            ct.ThrowIfCancellationRequested();

                            if (item.IsMatch)
                            {
                                IsSearchResultEmpty = false;
                                break;
                            }
                        }
                    }
                }, ct);
            }
            catch (OperationCanceledException e)
            {

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

                StartSearch();

            }
        }



        private async void StartSearch()
        {
            await doSearch();
        }
    }
}