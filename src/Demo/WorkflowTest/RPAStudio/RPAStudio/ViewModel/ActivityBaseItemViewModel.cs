using GalaSoft.MvvmLight;
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
    public class ActivityBaseItemViewModel : ViewModelBase
    {
        public int InitOrder { get; set; }//初始时的顺序，排序用，为在XML节点中的顺序


        //该枚举标记了分组类型，也标记了顺序
        public enum enGroupType
        {
            Null = 0,
            Favorites,
            Recent,
            Template,
            Activities
        }

        public enGroupType GroupType = enGroupType.Null;//标记当前节点的组类型

        /// <summary>
        /// Initializes a new instance of the ActivityBaseItemViewModel class.
        /// </summary>
        public ActivityBaseItemViewModel()
        {
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



        protected virtual string GetClassName()
        {
            return "";
        }



        /// <summary>
        /// The <see cref="IsExpanded" /> property's name.
        /// </summary>
        public const string IsExpandedPropertyName = "IsExpanded";

        private bool _isExpandedProperty = false;

        /// <summary>
        /// Sets and gets the IsExpanded property.
        /// Changes to that property's value raise the PropertyChanged event. 
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

        private ObservableCollection<ActivityBaseItemViewModel> _childrenProperty = new ObservableCollection<ActivityBaseItemViewModel>();

        /// <summary>
        /// Sets and gets the Children property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ActivityBaseItemViewModel> Children
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
        private void ActivityTreeItemSetAllIsMatch(ActivityBaseItemViewModel item, bool IsMatch)
        {
            item.IsMatch = IsMatch;
            foreach (var child in item.Children)
            {
                ActivityTreeItemSetAllIsMatch(child, IsMatch);
            }
        }

        /// <summary>
        /// 应用关键词
        /// </summary>
        /// <param name="criteria">关键词</param>
        /// <param name="ancestors">祖先链</param>
        public void ApplyCriteria(string criteria, Stack<ActivityBaseItemViewModel> ancestors)
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
                ActivityTreeItemSetAllIsMatch(this, true);
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
            bool isFound = string.IsNullOrEmpty(criteria) || Name.ContainsIgnoreCase(criteria);

            if (!isFound)
            {
                if (GetClassName().ContainsIgnoreCase(criteria))
                {
                    isFound = true;
                }
            }

            return isFound;
        }




    }
}