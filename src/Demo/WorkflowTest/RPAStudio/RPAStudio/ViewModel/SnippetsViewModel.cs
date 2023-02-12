using Activities.Shared.ActivityTemplateFactory;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using RPAStudio.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SnippetsViewModel : ViewModelBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IServiceLocator _serviceLocator;

        public SnippetItemDragHandler SnippetItemDragHandler { get; set; } = new SnippetItemDragHandler();

        /// <summary>
        /// Initializes a new instance of the SnippetsViewModel class.
        /// </summary>
        public SnippetsViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            Common.InvokeAsyncOnUI(() => {
                initSnippets();
            });

            Messenger.Default.Register<SnippetItemViewModel>(this, "RemoveSnippet", RemoveSnippet);
        }


        /// <summary>
        /// 移除片断
        /// </summary>
        /// <param name="obj">对象</param>
        private void RemoveSnippet(SnippetItemViewModel obj)
        {
            initSnippets();
        }

        /// <summary>
        /// 初始化组
        /// </summary>
        /// <param name="di">目录信息对象</param>
        /// <param name="parent">父对象</param>
        private void InitGroup(DirectoryInfo di, SnippetItemViewModel parent = null)
        {
            //当前目录文件夹遍历
            DirectoryInfo[] dis = di.GetDirectories();
            for (int j = 0; j < dis.Length; j++)
            {
                var item = _serviceLocator.ResolveType<SnippetItemViewModel>();
                item.ActivityFactoryAssemblyQualifiedName = InsertSnippetItemFactory.AssemblyQualifiedName;
                item.Path = dis[j].FullName;
                item.Name = dis[j].Name;

                if (parent != null)
                {
                    parent.Children.Add(item);
                }
                else
                {
                    item.IsExpanded = true;//默认展开第一层
                    SnippetsItems.Add(item);
                }

                InitGroup(dis[j], item);
            }

            //当前目录文件遍历
            FileInfo[] fis = di.GetFiles();
            for (int i = 0; i < fis.Length; i++)
            {
                var item = _serviceLocator.ResolveType<SnippetItemViewModel>();
                item.ActivityFactoryAssemblyQualifiedName = InsertSnippetItemFactory.AssemblyQualifiedName;
                item.IsSnippet = true;
                item.Path = fis[i].FullName;
                item.Name = fis[i].Name;

                if (fis[i].Extension.ToLower() == ProjectConstantConfig.XamlFileExtension)
                {
                    if (parent != null)
                    {
                        parent.Children.Add(item);
                    }
                    else
                    {
                        SnippetsItems.Add(item);
                    }
                }

            }

        }

        /// <summary>
        /// 初始化代码片断
        /// </summary>
        private void initSnippets()
        {
            //从文件夹中初始化Snippets,并按文件名排序
            SnippetsItems.Clear();
            DirectoryInfo di = new DirectoryInfo(Path.Combine(SharedObject.Instance.ApplicationCurrentDirectory, "CodeSnippets"));
            InitGroup(di);

            InitUserSnippets();
        }

        /// <summary>
        /// 初始化用户代码片断
        /// </summary>
        private void InitUserSnippets()
        {
            XmlDocument doc = new XmlDocument();
            var path = AppPathConfig.CodeSnippetsXml;
            doc.Load(path);
            var rootNode = doc.DocumentElement;

            var directoryNodes = rootNode.SelectNodes("Directory");
            foreach (XmlNode dir in directoryNodes)
            {
                var dirPath = (dir as XmlElement).GetAttribute("Path");
                var id = (dir as XmlElement).GetAttribute("Id");

                var userItem = _serviceLocator.ResolveType<SnippetItemViewModel>();
                userItem.ActivityFactoryAssemblyQualifiedName = InsertSnippetItemFactory.AssemblyQualifiedName;
                userItem.IsUserAdd = true;//标识为用户添加
                userItem.Id = id;
                userItem.Path = dirPath;
                userItem.Name = Path.GetFileName(dirPath);
                userItem.IsExpanded = true;

                SnippetsItems.Add(userItem);

                DirectoryInfo di = new DirectoryInfo(dirPath);
                InitGroup(di, userItem);
            }
        }


        /// <summary>
        /// The <see cref="SnippetsItems" /> property's name.
        /// </summary>
        public const string SnippetsItemsPropertyName = "SnippetsItems";

        private ObservableCollection<SnippetItemViewModel> _snippetsItemsProperty = new ObservableCollection<SnippetItemViewModel>();

        /// <summary>
        /// 代码片断条目
        /// </summary>
        public ObservableCollection<SnippetItemViewModel> SnippetsItems
        {
            get
            {
                return _snippetsItemsProperty;
            }

            set
            {
                if (_snippetsItemsProperty == value)
                {
                    return;
                }

                _snippetsItemsProperty = value;
                RaisePropertyChanged(SnippetsItemsPropertyName);
            }
        }

        /// <summary>
        /// 代码片断节点及所有子节点设置展开状态
        /// </summary>
        /// <param name="item">待设置的条目</param>
        /// <param name="IsExpanded">是否展开</param>
        private void SnippetTreeItemSetAllIsExpanded(SnippetItemViewModel item, bool IsExpanded)
        {
            item.IsExpanded = IsExpanded;
            foreach (var child in item.Children)
            {
                SnippetTreeItemSetAllIsExpanded(child, IsExpanded);
            }
        }

        /// <summary>
        /// 代码片断设置指定条目及所有子节点是否处在搜索中
        /// </summary>
        /// <param name="item">指定条目</param>
        /// <param name="IsSearching">是否搜索中</param>
        private void SnippetTreeItemSetAllIsSearching(SnippetItemViewModel item, bool IsSearching)
        {
            item.IsSearching = IsSearching;
            foreach (var child in item.Children)
            {
                SnippetTreeItemSetAllIsSearching(child, IsSearching);
            }
        }

        /// <summary>
        /// 代码片断设置指定节点及所有子节点是否匹配
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="IsMatch">是否匹配</param>
        private void SnippetTreeItemSetAllIsMatch(SnippetItemViewModel item, bool IsMatch)
        {
            item.IsMatch = IsMatch;
            foreach (var child in item.Children)
            {
                SnippetTreeItemSetAllIsMatch(child, IsMatch);
            }
        }

        /// <summary>
        /// 设置节点及所有子节点的搜索文本
        /// </summary>
        /// <param name="item">指定节点</param>
        /// <param name="SearchText">搜索文本</param>
        private void SnippetTreeItemSetAllSearchText(SnippetItemViewModel item, string SearchText)
        {
            item.SearchText = SearchText;
            foreach (var child in item.Children)
            {
                SnippetTreeItemSetAllSearchText(child, SearchText);
            }
        }


        private RelayCommand _expandAllCommand;

        /// <summary>
        /// 展开所有
        /// </summary>
        public RelayCommand ExpandAllCommand
        {
            get
            {
                return _expandAllCommand
                    ?? (_expandAllCommand = new RelayCommand(
                    () =>
                    {
                        foreach (var item in SnippetsItems)
                        {
                            SnippetTreeItemSetAllIsExpanded(item, true);
                        }
                    }));
            }
        }


        private RelayCommand _collapseAllCommand;

        /// <summary>
        /// 折叠所有
        /// </summary>
        public RelayCommand CollapseAllCommand
        {
            get
            {
                return _collapseAllCommand
                    ?? (_collapseAllCommand = new RelayCommand(
                    () =>
                    {
                        foreach (var item in SnippetsItems)
                        {
                            SnippetTreeItemSetAllIsExpanded(item, false);
                        }
                    }));
            }
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
                        initSnippets();
                    }));
            }
        }


        private RelayCommand _addFolderCommand;

        /// <summary>
        /// 添加目录
        /// </summary>
        public RelayCommand AddFolderCommand
        {
            get
            {
                return _addFolderCommand
                    ?? (_addFolderCommand = new RelayCommand(
                    () =>
                    {
                        //让用户选择欲添加代码片断的文件夹
                        string dst_dir = "";
                        if (CommonDialog.ShowSelectDirDialog("请选择一个目录添加到代码示例中", ref dst_dir))
                        {
                            //添加目录到配置文件中
                            XmlDocument doc = new XmlDocument();
                            var path = AppPathConfig.CodeSnippetsXml;
                            doc.Load(path);
                            var rootNode = doc.DocumentElement;

                            XmlElement dirElement = doc.CreateElement("Directory");
                            dirElement.SetAttribute("Id", System.Guid.NewGuid().ToString());
                            dirElement.SetAttribute("Path", dst_dir);

                            rootNode.AppendChild(dirElement);

                            doc.Save(path);

                            initSnippets();
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
        /// 执行实际的搜索
        /// </summary>
        private void doSearch()
        {
            var searchContent = SearchText ?? "";
            searchContent = searchContent.Trim();

            if (string.IsNullOrEmpty(searchContent))
            {
                //还原起始显示
                foreach (var item in SnippetsItems)
                {
                    SnippetTreeItemSetAllIsSearching(item, false);
                }

                foreach (var item in SnippetsItems)
                {
                    SnippetTreeItemSetAllSearchText(item, "");
                }

                IsSearchResultEmpty = false;
            }
            else
            {
                //根据搜索内容显示

                foreach (var item in SnippetsItems)
                {
                    SnippetTreeItemSetAllIsSearching(item, true);
                }

                //预先全部置为不匹配
                foreach (var item in SnippetsItems)
                {
                    SnippetTreeItemSetAllIsMatch(item, false);
                }


                foreach (var item in SnippetsItems)
                {
                    item.ApplyCriteria(searchContent, new Stack<SnippetItemViewModel>());
                }

                IsSearchResultEmpty = true;
                foreach (var item in SnippetsItems)
                {
                    if (item.IsMatch)
                    {
                        IsSearchResultEmpty = false;
                        break;
                    }
                }

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
    }


}