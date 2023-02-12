using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Shared.Utils;
using RPAStudio.Views;
using System.Text.RegularExpressions;
using System.Windows;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OutputListItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the OutputListItemViewModel class.
        /// </summary>
        public OutputListItemViewModel()
        {
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
        /// The <see cref="IsShowTimestamps" /> property's name.
        /// </summary>
        public const string IsShowTimestampsPropertyName = "IsShowTimestamps";

        private bool _isShowTimestampsProperty = false;

        /// <summary>
        /// 是否显示时间戳
        /// </summary>
        public bool IsShowTimestamps
        {
            get
            {
                return _isShowTimestampsProperty;
            }

            set
            {
                if (_isShowTimestampsProperty == value)
                {
                    return;
                }

                _isShowTimestampsProperty = value;
                RaisePropertyChanged(IsShowTimestampsPropertyName);

                updateToolTip();
            }
        }

        /// <summary>
        /// 更新提示信息
        /// </summary>
        private void updateToolTip()
        {
            if (IsShowTimestamps)
            {
                ToolTip = string.Format("日志时间：{0}\n日志内容：{1}", Timestamps, Msg);
            }
            else
            {
                ToolTip = string.Format("日志内容：{0}", Msg);
            }
        }



        /// <summary>
        /// The <see cref="Timestamps" /> property's name.
        /// </summary>
        public const string TimestampsPropertyName = "Timestamps";

        private string _timestampsProperty = "";

        /// <summary>
        /// 日志时间戳内容
        /// </summary>
        public string Timestamps
        {
            get
            {
                return _timestampsProperty;
            }

            set
            {
                if (_timestampsProperty == value)
                {
                    return;
                }

                _timestampsProperty = value;
                RaisePropertyChanged(TimestampsPropertyName);
            }
        }





        /// <summary>
        /// The <see cref="IsError" /> property's name.
        /// </summary>
        public const string IsErrorPropertyName = "IsError";

        private bool _isErrorProperty = false;

        /// <summary>
        /// 是否是错误日志
        /// </summary>
        public bool IsError
        {
            get
            {
                return _isErrorProperty;
            }

            set
            {
                if (_isErrorProperty == value)
                {
                    return;
                }

                _isErrorProperty = value;
                RaisePropertyChanged(IsErrorPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsWarning" /> property's name.
        /// </summary>
        public const string IsWarningPropertyName = "IsWarning";

        private bool _isWarningProperty = false;

        /// <summary>
        /// 是否是警告日志
        /// </summary>
        public bool IsWarning
        {
            get
            {
                return _isWarningProperty;
            }

            set
            {
                if (_isWarningProperty == value)
                {
                    return;
                }

                _isWarningProperty = value;
                RaisePropertyChanged(IsWarningPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsInformation" /> property's name.
        /// </summary>
        public const string IsInformationPropertyName = "IsInformation";

        private bool _isInformationProperty = false;

        /// <summary>
        /// 是否是信息日志
        /// </summary>
        public bool IsInformation
        {
            get
            {
                return _isInformationProperty;
            }

            set
            {
                if (_isInformationProperty == value)
                {
                    return;
                }

                _isInformationProperty = value;
                RaisePropertyChanged(IsInformationPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsTrace" /> property's name.
        /// </summary>
        public const string IsTracePropertyName = "IsTrace";

        private bool _isTraceProperty = false;

        /// <summary>
        /// 是否是跟踪日志
        /// </summary>
        public bool IsTrace
        {
            get
            {
                return _isTraceProperty;
            }

            set
            {
                if (_isTraceProperty == value)
                {
                    return;
                }

                _isTraceProperty = value;
                RaisePropertyChanged(IsTracePropertyName);
            }
        }




        /// <summary>
        /// The <see cref="Msg" /> property's name.
        /// </summary>
        public const string MsgPropertyName = "Msg";

        private string _msgProperty = "";

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg
        {
            get
            {
                return _msgProperty;
            }

            set
            {
                if (_msgProperty == value)
                {
                    return;
                }

                _msgProperty = value;
                RaisePropertyChanged(MsgPropertyName);

                updateToolTip();


            }
        }


        /// <summary>
        /// The <see cref="MsgDetails" /> property's name.
        /// </summary>
        public const string MsgDetailsPropertyName = "MsgDetails";

        private string _msgDetailsProperty = "";

        /// <summary>
        /// 消息详情
        /// </summary>
        public string MsgDetails
        {
            get
            {
                return _msgDetailsProperty;
            }

            set
            {
                if (_msgDetailsProperty == value)
                {
                    return;
                }

                _msgDetailsProperty = value;
                RaisePropertyChanged(MsgDetailsPropertyName);
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
        /// 是否正在搜索
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
        /// 应用搜索关键词
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
        /// 模糊搜索转正则
        /// </summary>
        /// <param name="value">模糊搜索内容，比如xx*xx</param>
        /// <returns>等效的正则表达式</returns>
        private static string wildCardToRegular(string value)
        {
            return ".*" + Regex.Escape(value).Replace("\\ ", ".*") + ".*";
        }

        /// <summary>
        /// 关键词是否匹配
        /// </summary>
        /// <param name="criteria">关键词</param>
        /// <returns>是否匹配</returns>
        private bool IsCriteriaMatched(string criteria)
        {
            return string.IsNullOrEmpty(criteria) || Regex.IsMatch(Msg, wildCardToRegular(criteria), RegexOptions.IgnoreCase);
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
                        Common.ShowContextMenu(this, "OutputItemContextMenu");
                    }));
            }
        }


        private RelayCommand _copyItemMsgCommand;

        /// <summary>
        /// 复制条目事件
        /// </summary>
        public RelayCommand CopyItemMsgCommand
        {
            get
            {
                return _copyItemMsgCommand
                    ?? (_copyItemMsgCommand = new RelayCommand(
                    () =>
                    {
                        Clipboard.SetDataObject(Msg);
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
                        ViewItemMsgDetailCommand.Execute(null);
                    }));
            }
        }


        private RelayCommand _viewItemMsgDetailCommand;

        /// <summary>
        /// 查看条目详情
        /// </summary>
        public RelayCommand ViewItemMsgDetailCommand
        {
            get
            {
                return _viewItemMsgDetailCommand
                    ?? (_viewItemMsgDetailCommand = new RelayCommand(
                    () =>
                    {
                        //弹出详细信息窗口
                        var window = new MessageDetailsWindow();

                        var vm = window.DataContext as MessageDetailsViewModel;
                        vm.WindowTitle = "消息详情";
                        vm.MsgDetails = MsgDetails;
                        CommonWindow.ShowDialog(window);
                    }));
            }
        }



    }
}