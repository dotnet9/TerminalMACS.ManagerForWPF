using GalaSoft.MvvmLight;
using NLog;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using System;
using System.Windows;
using System.Windows.Input;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DesignerDocumentViewModel : DocumentViewModel
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IAppDomainControllerService _appDomainControllerService;
        private IWorkflowDesignerServiceProxy _workflowDesignerServiceProxy;
        private IWorkflowDesignerCollectServiceProxy _workflowDesignerCollectServiceProxy;
        private IServiceLocator _serviceLocator;
        private IWorkflowStateService _workflowStateService;

        private ActivitiesViewModel _activitiesViewModel;

        public DesignerDocumentViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            _serviceLocator = serviceLocator;
            _appDomainControllerService = _serviceLocator.ResolveType<IAppDomainControllerService>();

            _workflowDesignerServiceProxy = _serviceLocator.ResolveType<IWorkflowDesignerServiceProxy>();
            _workflowDesignerCollectServiceProxy = _serviceLocator.ResolveType<IWorkflowDesignerCollectServiceProxy>();
            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();

            _activitiesViewModel = _serviceLocator.ResolveType<ActivitiesViewModel>();

            _workflowDesignerServiceProxy.ModelChangedEvent -= _workflowDesignerServiceProxy_ModelChangedEvent;
            _workflowDesignerServiceProxy.ModelChangedEvent += _workflowDesignerServiceProxy_ModelChangedEvent;

            _workflowDesignerServiceProxy.CanExecuteChanged -= _workflowDesignerServiceProxy_CanExecuteChanged;
            _workflowDesignerServiceProxy.CanExecuteChanged += _workflowDesignerServiceProxy_CanExecuteChanged;

            _workflowDesignerServiceProxy.ModelAddedEvent -= _workflowDesignerServiceProxy_ModelAddedEvent;
            _workflowDesignerServiceProxy.ModelAddedEvent += _workflowDesignerServiceProxy_ModelAddedEvent;

            _workflowStateService.BeginDebugEvent += _workflowStateService_BeginDebugEvent;
            _workflowStateService.EndDebugEvent += _workflowStateService_EndDebugEvent;

            _workflowStateService.BeginRunEvent += _workflowStateService_BeginRunEvent;
            _workflowStateService.EndRunEvent += _workflowStateService_EndRunEvent;
        }



        private void _workflowStateService_BeginRunEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsReadOnly = true;
            });
        }

        private void _workflowStateService_EndRunEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                IsReadOnly = false;
            });
        }

        private void _workflowStateService_BeginDebugEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                IsReadOnly = true;
            });
        }

        private void _workflowStateService_EndDebugEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                IsReadOnly = false;
            });
        }


        protected override void OnReadOnlySet(bool isReadOnly)
        {
            _workflowDesignerServiceProxy.SetReadOnly(isReadOnly);
        }

        private void _workflowDesignerServiceProxy_CanExecuteChanged(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void _workflowDesignerServiceProxy_ModelChangedEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() => {
                IsDirty = true;
            });
        }

        private void _workflowDesignerServiceProxy_ModelAddedEvent(object sender, string e)
        {
            Common.InvokeAsyncOnUI(() => {
                if (!string.IsNullOrEmpty(e))
                {
                    _activitiesViewModel.AddToRecent(e);
                }
            });
        }

        public IWorkflowDesignerServiceProxy GetWorkflowDesignerServiceProxy()
        {
            return _workflowDesignerServiceProxy;
        }


        /// <summary>
        /// The <see cref="WorkflowDesignerView" /> property's name.
        /// </summary>
        public const string WorkflowDesignerViewPropertyName = "WorkflowDesignerView";

        private FrameworkElement _workflowDesignerViewProperty = null;

        /// <summary>
        /// 工作流设计器视图
        /// </summary>
        public FrameworkElement WorkflowDesignerView
        {
            get
            {
                return _workflowDesignerViewProperty;
            }

            set
            {
                if (_workflowDesignerViewProperty == value)
                {
                    return;
                }

                _workflowDesignerViewProperty = value;
                RaisePropertyChanged(WorkflowDesignerViewPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="WorkflowPropertyView" /> property's name.
        /// </summary>
        public const string WorkflowPropertyViewPropertyName = "WorkflowPropertyView";

        private FrameworkElement _workflowPropertyViewProperty = null;

        /// <summary>
        /// 工作流属性视图
        /// </summary>
        public FrameworkElement WorkflowPropertyView
        {
            get
            {
                return _workflowPropertyViewProperty;
            }

            set
            {
                if (_workflowPropertyViewProperty == value)
                {
                    return;
                }

                _workflowPropertyViewProperty = value;
                RaisePropertyChanged(WorkflowPropertyViewPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="WorkflowOutlineView" /> property's name.
        /// </summary>
        public const string WorkflowOutlineViewPropertyName = "WorkflowOutlineView";

        private FrameworkElement _workflowOutlineViewProperty = null;

        /// <summary>
        /// Sets and gets the WorkflowOutlineView property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public FrameworkElement WorkflowOutlineView
        {
            get
            {
                return _workflowOutlineViewProperty;
            }

            set
            {
                if (_workflowOutlineViewProperty == value)
                {
                    return;
                }

                _workflowOutlineViewProperty = value;
                RaisePropertyChanged(WorkflowOutlineViewPropertyName);
            }
        }

        public void HideCurrentDebugArrow()
        {
            _workflowDesignerServiceProxy.HideCurrentLocation();
        }


        public void InsertActivity(string name, string assemblyQualifiedName)
        {
            _workflowDesignerServiceProxy.InsertActivity(name,assemblyQualifiedName);
        }


        public override void MakeView()
        {
            _workflowDesignerServiceProxy.Init(Path);

            WorkflowDesignerView = _workflowDesignerServiceProxy.GetDesignerView();
            WorkflowPropertyView = _workflowDesignerServiceProxy.GetPropertyView();
            WorkflowOutlineView = _workflowDesignerServiceProxy.GetOutlineView();
        }


        protected override void OnClose()
        {
            _workflowDesignerCollectServiceProxy.Remove(Path);
        }

        public override void Save()
        {
            //如果有只读属性，则需要判断下再保存
            if (IsReadOnly)
            {
                return;
            }

            try
            {
                _workflowDesignerServiceProxy.Save();
            }
            catch (Exception err)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, "保存工作流文档发生错误，请检查！", err);
                CommonMessageBox.ShowError("保存工作流文档发生错误，请检查！");
            }


            IsDirty = false;
        }

        protected override void FlushDesigner()
        {
            _workflowDesignerServiceProxy.FlushDesigner();
        }


        public override string XamlText
        {
            get
            {
                return _workflowDesignerServiceProxy.XamlText;
            }
        }


        public override void UpdatePathCrossDomain(string path)
        {
            _workflowDesignerServiceProxy.UpdatePath(path);
        }


        public override bool CanUndo()
        {
            return _workflowDesignerServiceProxy.CanUndo();
        }

        public override bool CanRedo()
        {
            return _workflowDesignerServiceProxy.CanRedo();
        }

        public override bool CanCut()
        {
            return _workflowDesignerServiceProxy.CanCut();
        }

        public override bool CanCopy()
        {
            return _workflowDesignerServiceProxy.CanCopy();
        }

        public override bool CanPaste()
        {
            return _workflowDesignerServiceProxy.CanPaste();
        }

        public override bool CanDelete()
        {
            return _workflowDesignerServiceProxy.CanDelete();
        }



        public override void Redo()
        {
            _workflowDesignerServiceProxy.Redo();
        }


        public override void Undo()
        {
            _workflowDesignerServiceProxy.Undo();
        }

        public override void Cut()
        {
            _workflowDesignerServiceProxy.Cut();
        }


        public override void Copy()
        {
            _workflowDesignerServiceProxy.Copy();
        }


        public override void Paste()
        {
            _workflowDesignerServiceProxy.Paste();
        }


        public override void Delete()
        {
            _workflowDesignerServiceProxy.Delete();
        }



        public override bool CanSave()
        {
            return true;
        }

    }
}