using Activities.Shared.ActivityTemplateFactory;
using Newtonsoft.Json.Linq;
using ReflectionMagic;
using RPA.Interfaces.Activities;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Project;
using RPA.Interfaces.Share;
using RPA.Interfaces.Workflow;
using RPA.Services.ExpressionEditor;
using RPA.Shared.Configs;
using RPA.Shared.Debugger;
using RPA.Shared.Utils;
using RPA.Shared.Workflow;
using System;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.Presentation.Debug;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Activities.Presentation.Validation;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xaml;

namespace RPA.Services.Workflow
{
    public class WorkflowDesignerService : MarshalByRefServiceBase, IWorkflowDesignerService
    {
        private IProjectManagerService _projectManagerService;
        private IActivitiesDefaultAttributesService _activitiesDefaultAttributesService;
        private IWorkflowDesignerCollectService _workflowDesignerCollectService;
        private IWorkflowBreakpointsService _workflowBreakpointsService;

        private WorkflowDesigner _designer;
        private FrameworkElement _view;

        private EventHandler _validationServiceValidationCompletedCallback;

        Dictionary<string, SourceLocation> _activityIdToSourceLocationMapping = new Dictionary<string, SourceLocation>();

        public string Path { get; private set; }

        private string _currentDraggingDisplayName = "";
        private string _currentDraggingAssemblyQualifiedName = "";
        private string _currentDraggingTypeOf = "";

        public event EventHandler ModelChangedEvent;
        public event EventHandler CanExecuteChanged;
        public event EventHandler<string> ModelAddedEvent;

        public WorkflowDesignerService(IProjectManagerService projectManagerService, IActivitiesDefaultAttributesService activitiesDefaultAttributesService
            ,IWorkflowDesignerCollectService workflowDesignerCollectService,IWorkflowBreakpointsService workflowBreakpointsService)
        {
            _projectManagerService = projectManagerService;
            _activitiesDefaultAttributesService = activitiesDefaultAttributesService;
            _activitiesDefaultAttributesService.Register();

            _workflowDesignerCollectService = workflowDesignerCollectService;
            _workflowDesignerCollectService.Add(this);

            _workflowBreakpointsService = workflowBreakpointsService;

            this._designer = new WorkflowDesigner();
            TryInitConfigurationService(_designer);

            this._designer.PropertyInspectorFontAndColorData = XamlServices.Save(GetThemeHashTable());//设计器样式设置

            _view = this._designer.View as FrameworkElement;

            _view.PreviewDragEnter += _view_PreviewDragEnter;

            DesignerView.UndoCommand.CanExecuteChanged += UndoCommand_CanExecuteChanged;
            DesignerView.RedoCommand.CanExecuteChanged += RedoCommand_CanExecuteChanged;
        }


        private void TryInitConfigurationService(WorkflowDesigner designer)
        {
            try
            {
                DesignerConfigurationService requiredService = designer.Context.Services.GetRequiredService<DesignerConfigurationService>();
                requiredService.TargetFrameworkName = new FrameworkName(".NETFramework,Version=v4.6.1");
                requiredService.MultipleItemsContextMenuEnabled = true;
                requiredService.LoadingFromUntrustedSourceEnabled = true;
            }
            catch (Exception err)
            {
                
            }
        }

        private object GetThemeHashTable()
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();

            resourceDictionary.Source = new Uri("pack://application:,,,/RPA.Resources;component/WorkflowDesigner/WorkflowDesignerStyle.xaml");

            return new Hashtable(resourceDictionary);
        }

        private void _view_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FilePath"))
            {
                var filePath = e.Data.GetData("FilePath") as string;

                if (e.Data.GetDataPresent("FileType"))
                {
                    var fileType = e.Data.GetData("FileType") as string;
                    if (fileType == "Xaml")
                    {
                        InvokeWorkflowFileFactory.FilePath = filePath;
                    }
                    else if (fileType == "Python")
                    {
                        InvokePythonFileFactory.FilePath = filePath;
                    }
                    else if (fileType == "Snippet")
                    {
                        InsertSnippetItemFactory.FilePath = filePath;
                    }
                }
            }


            if (e.Data.GetDataPresent("DisplayName"))
            {
                _currentDraggingDisplayName = e.Data.GetData("DisplayName") as string;
            }
            else
            {
                _currentDraggingDisplayName = "";
            }

            if (e.Data.GetDataPresent("AssemblyQualifiedName"))
            {
                _currentDraggingAssemblyQualifiedName = e.Data.GetData("AssemblyQualifiedName") as string;
            }
            else
            {
                _currentDraggingAssemblyQualifiedName = "";
            }

            if (e.Data.GetDataPresent("TypeOf"))
            {
                _currentDraggingTypeOf = e.Data.GetData("TypeOf") as string;
            }
            else
            {
                _currentDraggingTypeOf = "";
            }
        }

        private void RedoCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UndoCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Init(string path)
        {
            Path = path;
        }

        public void UpdatePath(string path)
        {
            Path = path;
        }

       

        public INativeHandleContract GetDesignerView()
        {
            PublishExpressionEditorService();

            var designerConfigurationService = _designer.Context.Services.GetService<DesignerConfigurationService>();

            designerConfigurationService.TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 6));//.NET版本设置
            designerConfigurationService.AutoSurroundWithSequenceEnabled = true;
            designerConfigurationService.AnnotationEnabled = true;

            this._designer.Load(Path);

            _view.Loaded -= _view_Loaded;
            _view.Loaded += _view_Loaded;

            return FrameworkElementAdapters.ViewToContractAdapter((FrameworkElement)this._designer.View);
        }

        private void _view_Loaded(object sender, RoutedEventArgs e)
        {
            var designerView = _designer.Context.Services.GetService<DesignerView>();

            designerView.WorkflowShellBarItemVisibility =
                ShellBarItemVisibility.All;

            SubscribeDesignerEvents();

            ShowBreakpointsOnValidationCompleted();
        }

        /// <summary>
        /// ValidationCompleted完成后断点信息才能正常获取
        /// </summary>
        private void ShowBreakpointsOnValidationCompleted()
        {
            ValidationService validationService = _designer.Context.Services.GetService<ValidationService>();
            if (validationService != null)
            {
                this._validationServiceValidationCompletedCallback = new EventHandler(this._validationServiceValidationComplete);
                EventInfo @event = validationService.GetType().GetEvent("ValidationCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
                EventHandler validationServiceValidationCompletedCallback = this._validationServiceValidationCompletedCallback;
                MethodBase addMethod = @event.GetAddMethod(true);
                object obj = validationService;
                object[] parameters = new EventHandler[]
                {
                    validationServiceValidationCompletedCallback
                };
                addMethod.Invoke(obj, parameters);
            }
        }

        private void _validationServiceValidationComplete(object sender, EventArgs e)
        {
            ShowBreakpoints();
        }

        private void SubscribeDesignerEvents()
        {
            var modelService = _designer.Context.Services.GetService<ModelService>();
            modelService.ModelChanged -= ModelService_ModelChanged;
            modelService.ModelChanged += ModelService_ModelChanged;

            _designer.ModelChanged -= _designer_ModelChanged;
            _designer.ModelChanged += _designer_ModelChanged;
        }

        /// <summary>
        /// 组件重命名后需要清空缓存信息，以便快捷键复制粘贴不出问题
        /// </summary>
        private void ClearCurrentDraggingInfo()
        {
            _currentDraggingDisplayName = "";
            _currentDraggingAssemblyQualifiedName = "";
            //_currentDraggingTypeOf = "";//不能清空，最近列表触发事件要用到
        }

        private void _designer_ModelChanged(object sender, EventArgs e)
        {
            ModelChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        public ModelItem GetRootModelItem(EditingContext context)
        {
            ModelTreeManager requiredService = context.Services.GetRequiredService<ModelTreeManager>();
            ModelItem modelItem = (requiredService != null) ? requiredService.Root : null;
            ActivityBuilder activityBuilder = ((modelItem != null) ? modelItem.GetCurrentValue() : null) as ActivityBuilder;
            if (activityBuilder == null)
            {
                return modelItem;
            }
            return requiredService.GetModelItem(activityBuilder.Implementation, false);
        }

        private ModelItem RootModelItem
        {
            get
            {
                return GetRootModelItem(this._designer.Context);
            }
        }

        public string XamlText
        {
            get
            {
                this._designer.Flush();
                return this._designer.Text;
            }
        }

        private void ModelService_ModelChanged(object sender, ModelChangedEventArgs e)
        {
            ModelChangedEvent?.Invoke(this, EventArgs.Empty);

            ModelChangeInfo modelChangeInfo = (e != null) ? e.ModelChangeInfo : null;
            ModelChangeType? modelChangeType = (modelChangeInfo != null) ? new ModelChangeType?(modelChangeInfo.ModelChangeType) : null;
            if (modelChangeType != null)
            {
                switch (modelChangeType.GetValueOrDefault())
                {
                    case ModelChangeType.PropertyChanged:
                        PropertyChanged(modelChangeInfo);

                        ModelAddedEvent?.Invoke(this, _currentDraggingTypeOf);
                        break;
                    case ModelChangeType.CollectionItemAdded:
                        CollectionItemAdded(modelChangeInfo);

                        ModelAddedEvent?.Invoke(this, _currentDraggingTypeOf);

                        break;
                    case ModelChangeType.CollectionItemRemoved:
                        return;
                    default:
                        return;
                }

                return;
            }
        }




        private void OnCollectionItemAdded(object obj, ModelItem modelItem)
        {
            OnRenameActivityDisplayName(obj, modelItem);

            OnSetActivityDefaultProperties(obj);
        }


        private void CollectionItemAdded(ModelChangeInfo changeInfo)
        {
            var obj = changeInfo.Value.GetCurrentValue();

            if (obj is Activity)
            {
                var activity = obj as Activity;

                OnCollectionItemAdded(activity, changeInfo.Value);
            }

            if (obj is FlowDecision)
            {
                var activity = obj as FlowDecision;

                OnCollectionItemAdded(activity, changeInfo.Value);
            }

            if (obj is FlowStep)
            {
                var flowStep = obj as FlowStep;
                var activity = flowStep.Action;
                OnCollectionItemAdded(activity, changeInfo.Value);
            }

            if (obj is FlowSwitch<object>)
            {
                var flowSwitch = obj as FlowSwitch<object>;
                var activity = flowSwitch;
                OnCollectionItemAdded(activity, changeInfo.Value);
            }

            if (obj is PickBranch)
            {
                var pickBranch = obj as PickBranch;
                var activity = pickBranch;
                OnCollectionItemAdded(activity, changeInfo.Value);
            }
        }


        public ModelItem SurroundItemWithSequence(ModelItem item, EditingContext context, string sequenceDisplayName)
        {
            ModelItem modelItem = null;
            using (ModelEditingScope modelEditingScope = item.BeginEdit())
            {
                modelItem = ModelFactory.CreateItem(context, new Sequence() { DisplayName = sequenceDisplayName });
                MorphHelper.MorphObject(item, modelItem);
                modelItem.Properties["Activities"].Collection.Add(item);
                modelEditingScope.Complete();
            }
            return modelItem;
        }

        private void PropertyChanged(ModelChangeInfo changeInfo)
        {
            if (changeInfo.PropertyName.Equals("Implementation"))
            {
                ModelItem rootModelItem = this.RootModelItem;
                if (rootModelItem != null)
                {
                    var sequenceDisplayName = _projectManagerService.ActivitiesTypeOfDict["Sequence"].Name;

                    if (rootModelItem.IsSequence())
                    {
                        DesignerWrapper.SetDisplayName(changeInfo.Value, sequenceDisplayName);
                    }

                    if (changeInfo.Value.Parent == changeInfo.Value.Root)
                    {
                        if (!changeInfo.Value.IsSequence() && !changeInfo.Value.IsFlowchart())
                        {
                            SurroundItemWithSequence(rootModelItem, this._designer.Context, sequenceDisplayName);
                        }
                    }
                }
            }

            if (changeInfo.Value == null)
            {
                return;
            }
            if (DesignerWrapper.IsSpecialProperty(changeInfo))
            {
                if (!string.IsNullOrEmpty(_currentDraggingDisplayName))
                {
                    DesignerWrapper.SetDisplayName(changeInfo.Value, _currentDraggingDisplayName);
                    ClearCurrentDraggingInfo();
                }
            }
        }

        public void ShowBreakpoints()
        {
            _workflowBreakpointsService.ShowBreakpoints(Path);
        }


        private void PublishExpressionEditorService()
        {
            //代码提示
            SyntaxService.CacheInFolder = AppPathConfig.StudioAppDataDir;
            _designer.Context.Services.Publish<IExpressionEditorService>(new ActiproExpressionEditorService(_designer.Context));
        }

        public INativeHandleContract GetPropertyView()
        {
            var view = this._designer.PropertyInspectorView as FrameworkElement;

            ///隐藏属性窗上方的命名空间Border
            FrameworkElement typeLabel = view.FindName("_typeLabel") as FrameworkElement;
            var border = (typeLabel.Parent as FrameworkElement).Parent as FrameworkElement;
            border.Visibility = Visibility.Collapsed;

            //隐藏属性窗上方的排序及搜索
            UIElement propertyToolBar = view.FindName("_propertyToolBar") as UIElement;
            if (propertyToolBar != null)
            {
                propertyToolBar.Visibility = Visibility.Collapsed;
            }

            return FrameworkElementAdapters.ViewToContractAdapter(view);
        }

        public INativeHandleContract GetOutlineView()
        {
            var view = this._designer.OutlineView as FrameworkElement;
            return FrameworkElementAdapters.ViewToContractAdapter(view);
        }

        public void Save()
        {
            _designer.Flush();
            var xamlText = _designer.Text;
            System.IO.File.WriteAllText(Path, xamlText);
        }

        public void FlushDesigner()
        {
            _designer.Flush();
        }

        public WorkflowDesigner GetWorkflowDesigner()
        {
            return _designer;
        }

        public bool CanUndo()
        {
            return DesignerView.UndoCommand.CanExecute(null);
        }

        public bool CanRedo()
        {
            return DesignerView.RedoCommand.CanExecute(null);
        }


        public bool CanCut()
        {
            return DesignerView.CutCommand.CanExecute(null);
        }

        public bool CanCopy()
        {
            return DesignerView.CopyCommand.CanExecute(null);
        }

        public bool CanPaste()
        {
            return DesignerView.PasteCommand.CanExecute(null);
        }

        public bool CanDelete()
        {
            return DesignerView.CutCommand.CanExecute(null);
        }

        public void Undo()
        {
            DesignerView.UndoCommand.Execute(null);
        }

        public void Redo()
        {
            DesignerView.RedoCommand.Execute(null);
        }


        public void Cut()
        {
            DesignerView.CutCommand.Execute(null);
        }

        public void Copy()
        {
            DesignerView.CopyCommand.Execute(null);
        }

        public void Paste()
        {
            DesignerView.PasteCommand.Execute(null);
        }

        public void Delete()
        {
            var selection = _designer.Context.Items.GetValue<Selection>();
            DesignerWrapper.RemoveActivity(selection.PrimarySelection);
        }

        public void ShowCurrentLocation(string locationId)
        {
            if (!_activityIdToSourceLocationMapping.ContainsKey(locationId))
            {
                return;
            }

            SourceLocation srcLoc = _activityIdToSourceLocationMapping[locationId];
            Common.InvokeAsyncOnUI(() =>
            {
                _designer.DebugManagerView.CurrentLocation = srcLoc;
            }, DispatcherPriority.Render);
        }

        public void HideCurrentLocation()
        {
            Common.InvokeAsyncOnUI(() =>
            {
                _designer.DebugManagerView.CurrentLocation = null;
            }, DispatcherPriority.Render);
        }

        public string GetActivityIdJsonArray()
        {
            RPADebugger.BuildSourceLocationMappings(_designer, ref _activityIdToSourceLocationMapping);

            JArray jarrayJsonObj = new JArray();
            foreach (var key in _activityIdToSourceLocationMapping.Keys.ToArray())
            {
                jarrayJsonObj.Add(key);
            }

            return jarrayJsonObj.ToString();
        }

        public string GetBreakpointIdJsonArray()
        {
            JArray breakpointIdJsonObj = new JArray();
            var breakpointLocations = _designer.DebugManagerView.GetBreakpointLocations();
            foreach (var item in _activityIdToSourceLocationMapping)
            {
                var id = item.Key;
                SourceLocation srcLoc = item.Value;
                if (breakpointLocations.ContainsKey(srcLoc))
                {
                    var types = breakpointLocations[srcLoc];
                    if (types == (BreakpointTypes.Enabled | BreakpointTypes.Bounded))
                    {
                        breakpointIdJsonObj.Add(id);
                    }
                }
            }

            return breakpointIdJsonObj.ToString();
        }

        public string GetTrackerVars()
        {
            List<string> varNameLsit = new List<string>();

            ModelService modelService = _designer.Context.Services.GetService<ModelService>();

            IEnumerable<ModelItem> flowcharts = modelService.Find(modelService.Root, typeof(Flowchart));
            IEnumerable<ModelItem> sequences = modelService.Find(modelService.Root, typeof(Sequence));

            foreach (var modelItem in flowcharts)
            {
                foreach (var varItem in modelItem.Properties["Variables"].Collection)
                {
                    var varName = varItem.Properties["Name"].ComputedValue as string;
                    varNameLsit.Add(varName);
                }
            }

            foreach (var modelItem in sequences)
            {
                foreach (var varItem in modelItem.Properties["Variables"].Collection)
                {
                    var varName = varItem.Properties["Name"].ComputedValue as string;
                    varNameLsit.Add(varName);
                }
            }

            JArray jarr = new JArray();
            foreach (var item in varNameLsit)
            {
                jarr.Add(item);
            }

            return jarr.ToString();
        }

        public void SetReadOnly(bool isReadOnly)
        {
            _designer.Context.Items.GetValue<ReadOnlyState>().IsReadOnly = isReadOnly;
            Common.InvokeAsyncOnUI(() => {
                var designView = _designer.Context.Services.GetService<DesignerView>();
                if (designView != null)
                {
                    designView.IsReadOnly = isReadOnly;
                }
                else
                {

                }
            });

        }



        /// <summary>
        /// 重命名活动组件
        /// </summary>
        /// <param name="obj"></param>
        private void OnRenameActivityDisplayName(object obj, ModelItem modelItem)
        {
            dynamic activity = obj;

            var assemblyQualifiedName = activity.GetType().AssemblyQualifiedName;
            if (assemblyQualifiedName == _currentDraggingAssemblyQualifiedName)
            {
                if (!string.IsNullOrEmpty(_currentDraggingDisplayName))
                {
                    DesignerWrapper.SetDisplayName(modelItem, _currentDraggingDisplayName);

                    ClearCurrentDraggingInfo();
                }
            }
            else
            {
                //如果包含WithBodyFactory则特殊处理下，直接通过
                if (_currentDraggingAssemblyQualifiedName.Contains("WithBodyFactory"))
                {
                    if (!string.IsNullOrEmpty(_currentDraggingDisplayName))
                    {
                        DesignerWrapper.SetDisplayName(modelItem, _currentDraggingDisplayName);

                        ClearCurrentDraggingInfo();
                    }
                }
            }
        }

        /// <summary>
        /// 设置活动组件的默认属性
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetActivityDefaultProperties(object obj)
        {
            if (obj is Delay)
            {
                var activity = obj as Delay;
                activity.Duration = TimeSpan.FromSeconds(3);
            }
        }



        public void RefreshArgumentsView()
        {
            DesignerView designerView = this._designer.Context.Services.GetService<DesignerView>();
            ContentControl contentControl = (ContentControl)designerView.FindName("arguments1");

            ((dynamic)contentControl.AsDynamic()).isCollectionLoaded = false;
            ((dynamic)contentControl.AsDynamic()).Populate();
        }

        public void UpdateCurrentSelecteddDesigner()
        {
            SharedVars.CurrentWorkflowDesignerService = this;
        }


        public void InsertActivity(string name, string assemblyQualifiedName)
        {
            //简单插入，只插入到属部，复杂的插入机制比如根据用户当前选择的焦点来插入可后期由用户自行扩展
            if (_designer != null)
            {
                ModelService modelService = _designer.Context.Services.GetService<ModelService>();
                ModelItem rootModelItem = modelService.Root.Properties["Implementation"].Value;

                var type = Type.GetType(assemblyQualifiedName);
                var activity = Activator.CreateInstance(type) as Activity;
                activity.DisplayName = name;

                if (rootModelItem == null)
                {
                    modelService.Root.Content.SetValue(activity);
                }
                else
                {
                    rootModelItem.AddActivity(activity);
                }
            }
        }
    }
}
