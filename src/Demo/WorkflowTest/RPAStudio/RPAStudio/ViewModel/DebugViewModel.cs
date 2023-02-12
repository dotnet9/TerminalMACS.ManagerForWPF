using GalaSoft.MvvmLight;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Shared.Executor;
using RPA.Shared.Utils;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DebugViewModel : ViewModelBase
    {
        private IServiceLocator _serviceLocator;
        private IWorkflowStateService _workflowStateService;
        private IProjectManagerService _projectManagerService;
        private DocksViewModel _docksViewModel;

        /// <summary>
        /// Initializes a new instance of the DebugViewModel class.
        /// </summary>
        public DebugViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();
            _projectManagerService = _serviceLocator.ResolveType<IProjectManagerService>();
            _docksViewModel = _serviceLocator.ResolveType<DocksViewModel>();

            _projectManagerService.ProjectPreviewOpenEvent += _projectManagerService_ProjectPreviewOpenEvent;
            _projectManagerService.ProjectPreviewCloseEvent += _projectManagerService_ProjectPreviewCloseEvent;

            _workflowStateService.ShowLocalsEvent += _workflowStateService_ShowLocalsEvent;

            _workflowStateService.BeginDebugEvent += _workflowStateService_BeginDebugEvent;
            _workflowStateService.EndDebugEvent += _workflowStateService_EndDebugEvent;
        }

        private void _workflowStateService_BeginDebugEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                TrackerItems.Clear();
            });
        }

        private void _workflowStateService_EndDebugEvent(object sender, EventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                TrackerItems.Clear();
            });
        }

        private void _projectManagerService_ProjectPreviewOpenEvent(object sender, EventArgs e)
        {

        }

        private void _projectManagerService_ProjectPreviewCloseEvent(object sender, CancelEventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                TrackerItems.Clear();
            });
        }



        /// <summary>
        /// 获取友好的类型名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>友好类型名</returns>
        public string GetFriendlyTypeName(Type type)
        {
            var codeDomProvider = CodeDomProvider.CreateProvider("C#");
            var typeReferenceExpression = new CodeTypeReferenceExpression(new CodeTypeReference(type));
            using (var writer = new StringWriter())
            {
                codeDomProvider.GenerateCodeFromExpression(typeReferenceExpression, writer, new CodeGeneratorOptions());
                return writer.GetStringBuilder().ToString();
            }
        }



        private void _workflowStateService_ShowLocalsEvent(object sender, object e)
        {
            if (e is ShowLocalsJsonMessage)
            {
                var showLocalsJsonMessage = e as ShowLocalsJsonMessage;

                var vars = showLocalsJsonMessage.Variables;
                var args = showLocalsJsonMessage.Arguments;

                Common.InvokeAsyncOnUI(() =>
                {
                    TrackerItems.Clear();
                    foreach (var item in vars)
                    {
                        var trackerItem = _serviceLocator.ResolveType<TrackerItemViewModel>();
                        trackerItem.Property = item.Key;
                        trackerItem.Value = item.Value;

                        TrackerItems.Add(trackerItem);
                    }

                    foreach (var item in args)
                    {
                        var trackerItem = _serviceLocator.ResolveType<TrackerItemViewModel>();
                        trackerItem.Property = item.Key;
                        trackerItem.Value = item.Value;

                        TrackerItems.Add(trackerItem);
                    }
                });
            }

        }


        /// <summary>
        /// The <see cref="TrackerItems" /> property's name.
        /// </summary>
        public const string TrackerItemsPropertyName = "TrackerItems";

        private ObservableCollection<TrackerItemViewModel> _trackerItemsProperty = new ObservableCollection<TrackerItemViewModel>();

        /// <summary>
        /// Sets and gets the TrackerItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<TrackerItemViewModel> TrackerItems
        {
            get
            {
                return _trackerItemsProperty;
            }

            set
            {
                if (_trackerItemsProperty == value)
                {
                    return;
                }

                _trackerItemsProperty = value;
                RaisePropertyChanged(TrackerItemsPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        private string _valueProperty = "";

        /// <summary>
        /// Sets and gets the Value property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Value
        {
            get
            {
                return _valueProperty;
            }

            set
            {
                if (_valueProperty == value)
                {
                    return;
                }

                _valueProperty = value;
                RaisePropertyChanged(ValuePropertyName);
            }
        }





    }
}