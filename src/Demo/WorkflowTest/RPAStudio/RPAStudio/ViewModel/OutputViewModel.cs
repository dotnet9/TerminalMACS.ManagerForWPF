using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OutputViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;
        private IProjectManagerService _projectManagerService;
        /// <summary>
        /// 对应的ListBox控件
        /// </summary>
        private ListBox _listBox;

        /// <summary>
        /// Initializes a new instance of the OutputViewModel class.
        /// </summary>
        public OutputViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();

            _projectManagerService.ProjectCloseEvent += _projectManagerService_ProjectCloseEvent;
        }

        private void _projectManagerService_ProjectCloseEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                ClearAllCommand.Execute(null);
            });
        }

        private RelayCommand<RoutedEventArgs> _listBoxLoadedCommand;

        /// <summary>
        /// ListBox加载完成时触发
        /// </summary>
        public RelayCommand<RoutedEventArgs> ListBoxLoadedCommand
        {
            get
            {
                return _listBoxLoadedCommand
                    ?? (_listBoxLoadedCommand = new RelayCommand<RoutedEventArgs>(
                    p =>
                    {
                        _listBox = (ListBox)p.Source;
                    }));
            }
        }






        /// <summary>
        /// The <see cref="IsShowTimestamps" /> property's name.
        /// </summary>
        public const string IsShowTimestampsPropertyName = "IsShowTimestamps";

        private bool _isShowTimestampsProperty = false;

        /// <summary>
        /// 是否显示时间信息
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
            }
        }




        /// <summary>
        /// The <see cref="IsShowError" /> property's name.
        /// </summary>
        public const string IsShowErrorPropertyName = "IsShowError";

        private bool _isShowErrorProperty = true;

        /// <summary>
        /// 是否显示错误日志
        /// </summary>
        public bool IsShowError
        {
            get
            {
                return _isShowErrorProperty;
            }

            set
            {
                if (_isShowErrorProperty == value)
                {
                    return;
                }

                _isShowErrorProperty = value;
                RaisePropertyChanged(IsShowErrorPropertyName);

                ShowItemsFilter(value, (item) => { return item.IsError; });
            }
        }



        /// <summary>
        /// The <see cref="IsShowWarning" /> property's name.
        /// </summary>
        public const string IsShowWarningPropertyName = "IsShowWarning";

        private bool _isShowWarningProperty = true;

        /// <summary>
        /// 是否显示警告日志
        /// </summary>
        public bool IsShowWarning
        {
            get
            {
                return _isShowWarningProperty;
            }

            set
            {
                if (_isShowWarningProperty == value)
                {
                    return;
                }

                _isShowWarningProperty = value;
                RaisePropertyChanged(IsShowWarningPropertyName);

                ShowItemsFilter(value, (item) => { return item.IsWarning; });
            }
        }

        /// <summary>
        /// The <see cref="IsShowInformation" /> property's name.
        /// </summary>
        public const string IsShowInformationPropertyName = "IsShowInformation";

        private bool _isShowInformationProperty = true;

        /// <summary>
        /// 是否显示信息日志
        /// </summary>
        public bool IsShowInformation
        {
            get
            {
                return _isShowInformationProperty;
            }

            set
            {
                if (_isShowInformationProperty == value)
                {
                    return;
                }

                _isShowInformationProperty = value;
                RaisePropertyChanged(IsShowInformationPropertyName);

                ShowItemsFilter(value, (item) => { return item.IsInformation; });
            }
        }

        /// <summary>
        /// The <see cref="IsShowTrace" /> property's name.
        /// </summary>
        public const string IsShowTracePropertyName = "IsShowTrace";

        private bool _isShowTraceProperty = true;

        /// <summary>
        /// 是否显示跟踪日志
        /// </summary>
        public bool IsShowTrace
        {
            get
            {
                return _isShowTraceProperty;
            }

            set
            {
                if (_isShowTraceProperty == value)
                {
                    return;
                }

                _isShowTraceProperty = value;
                RaisePropertyChanged(IsShowTracePropertyName);

                ShowItemsFilter(value, (item) => { return item.IsTrace; });
            }
        }



        /// <summary>
        /// The <see cref="ErrorCount" /> property's name.
        /// </summary>
        public const string ErrorCountPropertyName = "ErrorCount";

        private int _errorCountProperty = 0;

        /// <summary>
        /// 错误数量
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return _errorCountProperty;
            }

            set
            {
                if (_errorCountProperty == value)
                {
                    return;
                }

                _errorCountProperty = value;
                RaisePropertyChanged(ErrorCountPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="WarningCount" /> property's name.
        /// </summary>
        public const string WarningCountPropertyName = "WarningCount";

        private int _warningCountProperty = 0;

        /// <summary>
        /// 警告数量
        /// </summary>
        public int WarningCount
        {
            get
            {
                return _warningCountProperty;
            }

            set
            {
                if (_warningCountProperty == value)
                {
                    return;
                }

                _warningCountProperty = value;
                RaisePropertyChanged(WarningCountPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="InformationCount" /> property's name.
        /// </summary>
        public const string InformationCountPropertyName = "InformationCount";

        private int _informationCountProperty = 0;

        /// <summary>
        /// 信息数量
        /// </summary>
        public int InformationCount
        {
            get
            {
                return _informationCountProperty;
            }

            set
            {
                if (_informationCountProperty == value)
                {
                    return;
                }

                _informationCountProperty = value;
                RaisePropertyChanged(InformationCountPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="TraceCount" /> property's name.
        /// </summary>
        public const string TraceCountPropertyName = "TraceCount";

        private int _traceCountProperty = 0;

        /// <summary>
        /// 跟踪数量
        /// </summary>
        public int TraceCount
        {
            get
            {
                return _traceCountProperty;
            }

            set
            {
                if (_traceCountProperty == value)
                {
                    return;
                }

                _traceCountProperty = value;
                RaisePropertyChanged(TraceCountPropertyName);
            }
        }






        private RelayCommand _showTimestampsCommand;

        /// <summary>
        /// 显示时间信息
        /// </summary>
        public RelayCommand ShowTimestampsCommand
        {
            get
            {
                return _showTimestampsCommand
                    ?? (_showTimestampsCommand = new RelayCommand(
                    () =>
                    {
                        IsShowTimestamps = !IsShowTimestamps;

                        foreach (var item in OutputItems)
                        {
                            item.IsShowTimestamps = IsShowTimestamps;
                        }
                    }));
            }
        }




        private RelayCommand _showErrorCommand;

        /// <summary>
        /// 显示错误日志
        /// </summary>
        public RelayCommand ShowErrorCommand
        {
            get
            {
                return _showErrorCommand
                    ?? (_showErrorCommand = new RelayCommand(
                    () =>
                    {
                        IsShowError = !IsShowError;
                    }));
            }
        }



        private RelayCommand _showWarningCommand;

        /// <summary>
        /// 显示警告日志
        /// </summary>
        public RelayCommand ShowWarningCommand
        {
            get
            {
                return _showWarningCommand
                    ?? (_showWarningCommand = new RelayCommand(
                    () =>
                    {
                        IsShowWarning = !IsShowWarning;
                    }));
            }
        }

        private RelayCommand _showInformationCommand;

        /// <summary>
        /// 显示信息日志
        /// </summary>
        public RelayCommand ShowInformationCommand
        {
            get
            {
                return _showInformationCommand
                    ?? (_showInformationCommand = new RelayCommand(
                    () =>
                    {
                        IsShowInformation = !IsShowInformation;
                    }));
            }
        }


        private RelayCommand _showTraceCommand;

        /// <summary>
        /// 显示跟踪日志
        /// </summary>
        public RelayCommand ShowTraceCommand
        {
            get
            {
                return _showTraceCommand
                    ?? (_showTraceCommand = new RelayCommand(
                    () =>
                    {
                        IsShowTrace = !IsShowTrace;
                    }));
            }
        }



        private RelayCommand _clearAllCommand;

        /// <summary>
        /// 清空所有日志
        /// </summary>
        public RelayCommand ClearAllCommand
        {
            get
            {
                return _clearAllCommand
                    ?? (_clearAllCommand = new RelayCommand(
                    () =>
                    {
                        OutputItems.Clear();

                        ErrorCount = WarningCount = InformationCount = TraceCount = 0;
                    }));
            }
        }


        /// <summary>
        /// The <see cref="OutputItems" /> property's name.
        /// </summary>
        public const string OutputItemsPropertyName = "OutputItems";

        private ObservableCollection<OutputListItemViewModel> _outputItemsProperty = new ObservableCollection<OutputListItemViewModel>();

        /// <summary>
        /// 输出日志条目集合 
        /// </summary>
        public ObservableCollection<OutputListItemViewModel> OutputItems
        {
            get
            {
                return _outputItemsProperty;
            }

            set
            {
                if (_outputItemsProperty == value)
                {
                    return;
                }

                _outputItemsProperty = value;
                RaisePropertyChanged(OutputItemsPropertyName);
            }
        }


        private RelayCommand _copyItemMsgCommand;

        /// <summary>
        /// 复制条目信息
        /// </summary>
        public RelayCommand CopyItemMsgCommand
        {
            get
            {
                return _copyItemMsgCommand
                    ?? (_copyItemMsgCommand = new RelayCommand(
                    () =>
                    {
                        //获取选中的条目并执行拷贝命令
                        foreach (var item in OutputItems)
                        {
                            if (item.IsSelected)
                            {
                                item.CopyItemMsgCommand.Execute(null);
                                break;
                            }
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
        /// 执行实际的搜索功能
        /// </summary>
        private void doSearch()
        {
            string searchContent = SearchText ?? "";

            searchContent = searchContent.Trim();

            if (string.IsNullOrEmpty(searchContent))
            {
                //还原起始显示
                foreach (var item in OutputItems)
                {
                    item.IsSearching = false;
                }

                foreach (var item in OutputItems)
                {
                    item.SearchText = searchContent;
                }

                IsSearchResultEmpty = false;

                //搜索结果选中项在清除搜索结果时自动滚动到选中项，以方便使用
                _listBox.ScrollIntoView(_listBox.SelectedItem);
            }
            else
            {
                //根据搜索内容显示

                foreach (var item in OutputItems)
                {
                    item.IsSearching = true;
                }

                //预先全部置为不匹配
                foreach (var item in OutputItems)
                {
                    item.IsMatch = false;
                }


                foreach (var item in OutputItems)
                {
                    item.ApplyCriteria(searchContent);
                }

                IsSearchResultEmpty = true;
                foreach (var item in OutputItems)
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
        /// 日志记录
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="msg">消息</param>
        /// <param name="msgDetails">消息详情</param>
        public void Log(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            //如果消息详情为空，则默认详情复用消息的内容
            if (string.IsNullOrEmpty(msgDetails))
            {
                msgDetails = msg;
            }


            var item = _serviceLocator.ResolveType<OutputListItemViewModel>();

            switch (type)
            {
                case SharedObject.enOutputType.Error:
                    item.IsError = true;
                    ErrorCount++;
                    break;
                case SharedObject.enOutputType.Information:
                    item.IsInformation = true;
                    InformationCount++;
                    break;
                case SharedObject.enOutputType.Warning:
                    item.IsWarning = true;
                    WarningCount++;
                    break;
                case SharedObject.enOutputType.Trace:
                    item.IsTrace = true;
                    TraceCount++;
                    break;
                default:
                    break;
            }

            item.IsShowTimestamps = IsShowTimestamps;
            item.Timestamps = "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "]";
            item.Msg = msg;
            item.MsgDetails = msgDetails;
            OutputItems.Add(item);

            doSearch();
        }

        /// <summary>
        /// 显示日志条目，带过滤功能
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="compare"></param>
        private void ShowItemsFilter(bool isVisible, Func<OutputListItemViewModel, bool> compare)
        {
            foreach (var item in OutputItems)
            {
                if (compare(item))
                {
                    item.IsVisible = isVisible;
                }
            }
        }




    }
}