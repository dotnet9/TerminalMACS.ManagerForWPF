using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Shared.Utils;
using System.Windows.Media;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocumentViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;
        private DocksViewModel _docksViewModel;
        private IWorkflowStateService _workflowStateService;
        private MainViewModel _mainViewModel;

        /// <summary>
        /// Initializes a new instance of the DocumentViewModel class.
        /// </summary>
        public DocumentViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _docksViewModel = _serviceLocator.ResolveType<DocksViewModel>();
            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();

            _mainViewModel = _serviceLocator.ResolveType<MainViewModel>();

        }

        public virtual void MakeView()
        {

        }

        protected virtual string OnProcessTitle(string name)
        {
            return name;
        }



        /// <summary>
        /// The <see cref="IsOpen" /> property's name.
        /// </summary>
        public const string IsOpenPropertyName = "IsOpen";

        private bool _isOpenProperty = true;//ACTIPRO的文档创建时需要设置为true,否则不显示

        /// <summary>
        /// Sets and gets the IsOpen property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return _isOpenProperty;
            }

            set
            {
                if (_isOpenProperty == value)
                {
                    return;
                }

                _isOpenProperty = value;
                RaisePropertyChanged(IsOpenPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="IsReadOnly" /> property's name.
        /// </summary>
        public const string IsReadOnlyPropertyName = "IsReadOnly";

        private bool _isReadOnlyProperty = false;

        /// <summary>
        /// Sets and gets the IsReadOnly property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnlyProperty;
            }

            set
            {
                if (IsAlwaysReadOnly && !value)
                {
                    //处于一直只读状态时，不允许变回非只读状态
                    return;
                }

                if (_isReadOnlyProperty == value)
                {
                    return;
                }

                _isReadOnlyProperty = value;
                RaisePropertyChanged(IsReadOnlyPropertyName);

                if (value)
                {
                    ReadOnlyDescription = "(只读)";
                }
                else
                {
                    ReadOnlyDescription = "";
                }

                OnReadOnlySet(value);
            }
        }



        /// <summary>
        /// The <see cref="IsAlwaysReadOnly" /> property's name.
        /// </summary>
        public const string IsAlwaysReadOnlyPropertyName = "IsAlwaysReadOnly";

        private bool _isAlwaysReadOnlyProperty = false;

        /// <summary>
        /// 设置是否是一直只读状态的文件，比如代码片断
        /// 处于一直只读状态时，即使运行或调试结束时，依然是只读状态
        /// </summary>
        public bool IsAlwaysReadOnly
        {
            get
            {
                return _isAlwaysReadOnlyProperty;
            }

            set
            {
                if (_isAlwaysReadOnlyProperty == value)
                {
                    return;
                }

                _isAlwaysReadOnlyProperty = value;
                RaisePropertyChanged(IsAlwaysReadOnlyPropertyName);

                IsReadOnly = value;
            }
        }





        /// <summary>
        /// The <see cref="IsShowTitle" /> property's name.
        /// </summary>
        public const string IsShowTitlePropertyName = "IsShowTitle";

        private bool _isShowTitleProperty = true;

        /// <summary>
        /// Sets and gets the IsShowTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsShowTitle
        {
            get
            {
                return _isShowTitleProperty;
            }

            set
            {
                if (_isShowTitleProperty == value)
                {
                    return;
                }

                _isShowTitleProperty = value;
                RaisePropertyChanged(IsShowTitlePropertyName);
            }
        }



        /// <summary>
        /// The <see cref="IsShowIcon" /> property's name.
        /// </summary>
        public const string IsShowIconPropertyName = "IsShowIcon";

        private bool _isShowIconProperty = true;

        /// <summary>
        /// Sets and gets the IsShowIcon property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsShowIcon
        {
            get
            {
                return _isShowIconProperty;
            }

            set
            {
                if (_isShowIconProperty == value)
                {
                    return;
                }

                _isShowIconProperty = value;
                RaisePropertyChanged(IsShowIconPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _titleProperty = "";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _titleProperty;
            }

            set
            {
                if (_titleProperty == value)
                {
                    return;
                }

                _titleProperty = OnProcessTitle(value);
                RaisePropertyChanged(TitlePropertyName);
            }
        }



        /// <summary>
        /// The <see cref="Icon" /> property's name.
        /// </summary>
        public const string IconPropertyName = "Icon";

        private string _iconProperty = null;

        /// <summary>
        /// Sets and gets the Icon property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Icon
        {
            get
            {
                return _iconProperty;
            }

            set
            {
                if (_iconProperty == value)
                {
                    return;
                }

                _iconProperty = value;
                RaisePropertyChanged(IconPropertyName);

                IconSource = Common.ToImageSource(value);
            }
        }


        /// <summary>
        /// The <see cref="IconSource" /> property's name.
        /// </summary>
        public const string IconSourcePropertyName = "IconSource";

        private ImageSource _iconSourceProperty = null;

        /// <summary>
        /// Sets and gets the IconSource property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ImageSource IconSource
        {
            get
            {
                return _iconSourceProperty;
            }

            set
            {
                if (_iconSourceProperty == value)
                {
                    return;
                }

                _iconSourceProperty = value;
                RaisePropertyChanged(IconSourcePropertyName);
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


        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        private bool _isSelectedProperty = false;

        /// <summary>
        /// Sets and gets the IsSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
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

                if (value)
                {
                    _docksViewModel.RaiseDocumentSelectChangeEvent(this);
                }
                else
                {
                    //此处刷新下设计器，避免在参数编辑状态中切换TAB导致异常卡死的问题
                    FlushDesigner();
                }
            }
        }


        /// <summary>
        /// The <see cref="IsActive" /> property's name.
        /// </summary>
        public const string IsActivePropertyName = "IsActive";

        private bool _isActiveProperty = false;

        /// <summary>
        /// Sets and gets the IsActive property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActiveProperty;
            }

            set
            {
                if (_isActiveProperty == value)
                {
                    return;
                }

                _isActiveProperty = value;
                RaisePropertyChanged(IsActivePropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsFloating" /> property's name.
        /// </summary>
        public const string IsFloatingPropertyName = "IsFloating";

        private bool _isFloatingProperty = false;

        /// <summary>
        /// Sets and gets the IsFloating property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsFloating
        {
            get
            {
                return _isFloatingProperty;
            }

            set
            {
                if (_isFloatingProperty == value)
                {
                    return;
                }

                _isFloatingProperty = value;
                RaisePropertyChanged(IsFloatingPropertyName);
            }
        }



        protected virtual void FlushDesigner()
        {

        }


        private void RealClose()
        {
            IsDirty = false;//真实关闭时，脏标志清空

            var _docksViewModel = _serviceLocator.ResolveType<DocksViewModel>();
            _docksViewModel.Documents.Remove(this);

            OnClose();

            if (_docksViewModel.Documents.Count == 0)
            {
                _docksViewModel.RaiseDocumentSelectChangeEvent(null);
            }
        }


        private RelayCommand _closeCommand;

        /// <summary>
        /// Gets the CloseCommand.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand
                    ?? (_closeCommand = new RelayCommand(
                    () =>
                    {
                        CloseQuery();
                    }));
            }
        }


        protected virtual void OnClose()
        {

        }

        protected virtual void OnReadOnlySet(bool isReadOnly)
        {

        }


        /// <summary>
        /// 文件相对于项目的相对路径
        /// </summary>
        /// <summary>
        /// The <see cref="RelativePath" /> property's name.
        /// </summary>
        public const string RelativePathPropertyName = "RelativePath";

        private string _relativePathProperty = "";

        /// <summary>
        /// Sets and gets the RelativePath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RelativePath
        {
            get
            {
                return _relativePathProperty;
            }

            set
            {
                if (_relativePathProperty == value)
                {
                    return;
                }

                _relativePathProperty = value;
                RaisePropertyChanged(RelativePathPropertyName);
            }
        }

        public virtual void UpdatePathCrossDomain(string path)
        {

        }

        public bool CloseQuery()
        {
            //调试当前文件时的关闭询问
            if (_workflowStateService.IsDebugging && _workflowStateService.RunningOrDebuggingFile == this.Path)
            {
                var ret = CommonMessageBox.ShowQuestion(string.Format("当前文档正在被调试，确定终止调试并关闭\"{0}\"吗？", Path));
                if (ret)
                {
                    _mainViewModel.StopWorkflowCommand.Execute(null);
                }
                else
                {
                    return false;
                }
            }

            //当前文档窗口关闭
            bool isClose = true;

            if (IsDirty)
            {
                bool? ret = CommonMessageBox.ShowQuestionYesNoCancel(string.Format("文件有修改，需要保存文件\"{0}\"吗？", Path), true);

                if (ret == true)
                {
                    Save();
                }
                else if (ret == false)
                {

                }
                else
                {
                    isClose = false;
                }
            }

            if (isClose)
            {
                RealClose();
            }


            return isClose;
        }

        public virtual void Save()
        {

        }


        public virtual string XamlText
        {
            get;
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
        /// The <see cref="ReadOnlyDescription" /> property's name.
        /// </summary>
        public const string ReadOnlyDescriptionPropertyName = "ReadOnlyDescription";

        private string _readOnlyDescriptionProperty = "";

        /// <summary>
        /// Sets and gets the ReadOnlyDescription property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ReadOnlyDescription
        {
            get
            {
                return _readOnlyDescriptionProperty;
            }

            set
            {
                if (_readOnlyDescriptionProperty == value)
                {
                    return;
                }

                _readOnlyDescriptionProperty = value;
                RaisePropertyChanged(ReadOnlyDescriptionPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="DirtyStar" /> property's name.
        /// </summary>
        public const string DirtyStarPropertyName = "DirtyStar";

        private string _dirtyStarProperty = "";

        /// <summary>
        /// Sets and gets the DirtyStar property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DirtyStar
        {
            get
            {
                return _dirtyStarProperty;
            }

            set
            {
                if (_dirtyStarProperty == value)
                {
                    return;
                }

                _dirtyStarProperty = value;
                RaisePropertyChanged(DirtyStarPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsDirty" /> property's name.
        /// </summary>
        public const string IsDirtyPropertyName = "IsDirty";

        private bool _isDirtyProperty = false;

        /// <summary>
        /// Sets and gets the IsDirty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return _isDirtyProperty;
            }

            set
            {
                if (_isDirtyProperty == value)
                {
                    return;
                }

                _isDirtyProperty = value;
                RaisePropertyChanged(IsDirtyPropertyName);

                if (value)
                {
                    DirtyStar = "*";
                }
                else
                {
                    DirtyStar = "";
                }
            }
        }



        public virtual bool CanUndo()
        {
            return false;
        }

        public virtual bool CanRedo()
        {
            return false;
        }

        public virtual bool CanCut()
        {
            return false;
        }

        public virtual bool CanCopy()
        {
            return false;
        }

        public virtual bool CanPaste()
        {
            return false;
        }

        public virtual bool CanDelete()
        {
            return false;
        }


        public virtual void Redo()
        {

        }


        public virtual void Undo()
        {

        }

        public virtual void Cut()
        {

        }

        public virtual void Copy()
        {

        }

        public virtual void Paste()
        {

        }

        public virtual void Delete()
        {

        }


        public virtual bool CanSave()
        {
            return false;
        }

    }
}