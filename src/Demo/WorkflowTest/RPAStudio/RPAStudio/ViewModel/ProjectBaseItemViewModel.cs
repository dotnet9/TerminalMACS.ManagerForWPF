using GalaSoft.MvvmLight;
using RPA.Interfaces.Service;
using RPA.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProjectBaseItemViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;
        private ProjectViewModel _projectViewModel;
        /// <summary>
        /// Initializes a new instance of the ProjectBaseItemViewModel class.
        /// </summary>
        public ProjectBaseItemViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _projectViewModel = _serviceLocator.ResolveType<ProjectViewModel>();
        }


        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _nameProperty = "";

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
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
        /// The <see cref="Path" /> property's name.
        /// </summary>
        public const string PathPropertyName = "Path";

        private string _pathProperty = "";

        /// <summary>
        /// Sets and gets the Path property.
        /// Changes to that property's value raise the PropertyChanged event. 
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
                if (_projectViewModel.IsExpandedDict.ContainsKey(Path))
                {
                    return _projectViewModel.IsExpandedDict[Path];
                }

                return _isExpandedProperty;
            }

            set
            {
                if (!string.IsNullOrEmpty(Path))
                {
                    _projectViewModel.IsExpandedDict[Path] = value;
                }


                if (_isExpandedProperty == value)
                {
                    return;
                }

                _isExpandedProperty = value;
                RaisePropertyChanged(IsExpandedPropertyName);
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
        /// The <see cref="Children" /> property's name.
        /// </summary>
        public const string ChildrenPropertyName = "Children";

        private ObservableCollection<ProjectBaseItemViewModel> _childrenProperty = new ObservableCollection<ProjectBaseItemViewModel>();

        /// <summary>
        /// Sets and gets the Children property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ProjectBaseItemViewModel> Children
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
        /// 设置指定节点及所有子节点的是否匹配状态
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
        /// 应用关键词
        /// </summary>
        /// <param name="criteria">关键词</param>
        /// <param name="ancestors">祖先链</param>
        public void ApplyCriteria(string criteria, Stack<ProjectBaseItemViewModel> ancestors)
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
                ProjectTreeItemSetAllIsMatch(this, true);
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
            return String.IsNullOrEmpty(criteria) || Name.ContainsIgnoreCase(criteria);
        }


    }
}