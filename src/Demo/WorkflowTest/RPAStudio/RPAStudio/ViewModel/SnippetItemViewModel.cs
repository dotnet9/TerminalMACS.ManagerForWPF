using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RPA.Shared.Configs;
using RPA.Shared.Extensions;
using RPA.Shared.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SnippetItemViewModel : ViewModelBase
    {
        public string ActivityFactoryAssemblyQualifiedName { get; set; }

        private DocksViewModel _docksViewModel;

        /// <summary>
        /// Initializes a new instance of the SnippetItemViewModel class.
        /// </summary>
        public SnippetItemViewModel(DocksViewModel docksViewModel)
        {
            _docksViewModel = docksViewModel;
        }




        /// <summary>
        /// The <see cref="Id" /> property's name.
        /// </summary>
        public const string IdPropertyName = "Id";

        private string _idProperty = System.Guid.NewGuid().ToString();

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id
        {
            get
            {
                return _idProperty;
            }

            set
            {
                if (_idProperty == value)
                {
                    return;
                }

                _idProperty = value;
                RaisePropertyChanged(IdPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsExpanded" /> property's name.
        /// </summary>
        public const string IsExpandedPropertyName = "IsExpanded";

        private bool _isExpandedProperty = false;

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return _isExpandedProperty;
            }

            set
            {
                if (_isExpandedProperty == value)
                {
                    return;
                }

                _isExpandedProperty = value;
                RaisePropertyChanged(IsExpandedPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="Children" /> property's name.
        /// </summary>
        public const string ChildrenPropertyName = "Children";

        private ObservableCollection<SnippetItemViewModel> _childrenProperty = new ObservableCollection<SnippetItemViewModel>();

        /// <summary>
        /// 子节点
        /// </summary>
        public ObservableCollection<SnippetItemViewModel> Children
        {
            get
            {
                return _childrenProperty;
            }

            set
            {
                if (_childrenProperty == value)
                {
                    return;
                }

                _childrenProperty = value;
                RaisePropertyChanged(ChildrenPropertyName);
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
        /// The <see cref="IsSnippet" /> property's name.
        /// </summary>
        public const string IsSnippetPropertyName = "IsSnippet";

        private bool _isSnippetProperty = false;

        /// <summary>
        /// 是否是代码片断
        /// </summary>
        public bool IsSnippet
        {
            get
            {
                return _isSnippetProperty;
            }

            set
            {
                if (_isSnippetProperty == value)
                {
                    return;
                }

                _isSnippetProperty = value;
                RaisePropertyChanged(IsSnippetPropertyName);
            }
        }


        /// <summary>
        /// 是否是用户手动添加进来的目录，右键菜单有移除选项
        /// </summary>
        public const string IsUserAddPropertyName = "IsUserAdd";

        private bool _isUserAddProperty = false;

        /// <summary>
        /// Sets and gets the IsUserAdd property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsUserAdd
        {
            get
            {
                return _isUserAddProperty;
            }

            set
            {
                if (_isUserAddProperty == value)
                {
                    return;
                }

                _isUserAddProperty = value;
                RaisePropertyChanged(IsUserAddPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="Path" /> property's name.
        /// </summary>
        public const string PathPropertyName = "Path";

        private string _pathProperty = "";

        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get
            {
                return _pathProperty;
            }

            set
            {
                if (_pathProperty == value)
                {
                    return;
                }

                _pathProperty = value;
                RaisePropertyChanged(PathPropertyName);
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


        private RelayCommand<MouseButtonEventArgs> _treeNodeMouseRightButtonUpCommand;

        /// <summary>
        /// 树节点鼠标右键松开
        /// </summary>
        public RelayCommand<MouseButtonEventArgs> TreeNodeMouseRightButtonUpCommand
        {
            get
            {
                return _treeNodeMouseRightButtonUpCommand
                    ?? (_treeNodeMouseRightButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(
                    p =>
                    {
                        //控件右击动态弹出菜单
                        if (IsUserAdd && !IsSnippet)
                        {
                            Common.ShowContextMenu(this, "SnippetItemUserAddContextMenu");
                        }
                        else
                        {
                            Common.ShowContextMenu(this, "SnippetItemContextMenu");
                        }
                    }));
            }
        }


        private RelayCommand<MouseButtonEventArgs> _treeNodeMouseDoubleClickCommand;

        /// <summary>
        /// 树节点鼠标双击
        /// </summary>
        public RelayCommand<MouseButtonEventArgs> TreeNodeMouseDoubleClickCommand
        {
            get
            {
                return _treeNodeMouseDoubleClickCommand
                    ?? (_treeNodeMouseDoubleClickCommand = new RelayCommand<MouseButtonEventArgs>(
                    p =>
                    {
                        if (IsSnippet)
                        {
                            OpenSnippetCommand.Execute(null);
                        }

                    }));
            }
        }



        /// <summary>
        /// 设置指定节点和所有子节点是否匹配
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
        /// 应用关键词
        /// </summary>
        /// <param name="criteria">关键词</param>
        /// <param name="ancestors">祖先链</param>
        public void ApplyCriteria(string criteria, Stack<SnippetItemViewModel> ancestors)
        {
            SearchText = criteria;

            if (IsCriteriaMatched(criteria))
            {
                IsMatch = true;
                IsExpanded = true;

                foreach (var ancestor in ancestors)
                {
                    ancestor.IsMatch = true;
                    ancestor.IsExpanded = true;
                }

                //如果是组名匹配，则下面的子节点和子子等节点要把IsMatch都设置为true
                SnippetTreeItemSetAllIsMatch(this, true);
            }

            ancestors.Push(this);
            foreach (var child in Children)
                child.ApplyCriteria(criteria, ancestors);

            ancestors.Pop();
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




        private RelayCommand _openSnippetCommand;

        /// <summary>
        /// 打开代码片断到设计器中
        /// </summary>
        public RelayCommand OpenSnippetCommand
        {
            get
            {
                return _openSnippetCommand
                    ?? (_openSnippetCommand = new RelayCommand(
                    () =>
                    {
                        if (IsSnippet)
                        {
                            //打开代码片断文件
                            //判断是否存在，若存在，直接切换，否则打开新文档

                            DesignerDocumentViewModel doc;
                            bool isExist = _docksViewModel.IsDocumentExist(Path, out doc);

                            if (!isExist)
                            {
                                var newDoc = _docksViewModel.NewDesignerDocument(Path);
                                newDoc.IsAlwaysReadOnly = true;
                            }
                            else
                            {
                                doc.IsSelected = true;
                            }
                        }
                        else
                        {
                            //打开文件夹
                            Common.LocateDirInExplorer(Path);
                        }
                    }));
            }
        }



        private RelayCommand _removeSnippetCommand;

        /// <summary>
        /// 移除代码片断
        /// </summary>
        public RelayCommand RemoveSnippetCommand
        {
            get
            {
                return _removeSnippetCommand
                    ?? (_removeSnippetCommand = new RelayCommand(
                    () =>
                    {
                        //移除用户选择的目录
                        XmlDocument doc = new XmlDocument();
                        var path = AppPathConfig.CodeSnippetsXml;
                        var rootNode = doc.DocumentElement;

                        var directoryNodes = rootNode.SelectNodes("Directory");
                        foreach (XmlNode dir in directoryNodes)
                        {
                            var id = (dir as XmlElement).GetAttribute("Id");

                            if (Id == id)
                            {
                                rootNode.RemoveChild(dir);
                                break;
                            }
                        }

                        doc.Save(path);

                        //发消息通知代码片断视图刷新树节点，也可以考虑用事件绑定机制来处理
                        Messenger.Default.Send(this, "RemoveSnippet");
                    }));
            }
        }



        private RelayCommand<MouseEventArgs> _treeNodeMouseMoveCommand;

        /// <summary>
        /// 鼠标在树节点上移动
        /// </summary>
        public RelayCommand<MouseEventArgs> TreeNodeMouseMoveCommand
        {
            get
            {
                return _treeNodeMouseMoveCommand
                    ?? (_treeNodeMouseMoveCommand = new RelayCommand<MouseEventArgs>(
                    p =>
                    {

                    }));
            }
        }



    }
}