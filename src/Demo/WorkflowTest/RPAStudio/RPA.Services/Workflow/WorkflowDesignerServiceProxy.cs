using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Workflow;
using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Services.Workflow
{
    public class WorkflowDesignerServiceProxy : MarshalByRefServiceProxyBase<IWorkflowDesignerService>, IWorkflowDesignerServiceProxy
    {
        public string XamlText
        {
            get
            {
                return InnerService.XamlText;
            }
        }

        public event EventHandler ModelChangedEvent;
        public event EventHandler CanExecuteChanged;
        public event EventHandler<string> ModelAddedEvent;

        public WorkflowDesignerServiceProxy(IAppDomainControllerService appDomainControllerService) : base(appDomainControllerService)
        {

        }

        public void Init(string path)
        {
            InnerService.Init(path);
        }

        public void UpdatePath(string path)
        {
            InnerService.UpdatePath(path);
        }

        public FrameworkElement GetDesignerView()
        {
            return FrameworkElementAdapters.ContractToViewAdapter(InnerService.GetDesignerView());
        }

        public FrameworkElement GetPropertyView()
        {
            return FrameworkElementAdapters.ContractToViewAdapter(InnerService.GetPropertyView());
        }

        public FrameworkElement GetOutlineView()
        {
            return FrameworkElementAdapters.ContractToViewAdapter(InnerService.GetOutlineView());
        }

        protected override void OnAfterConnectToInnerService()
        {
            InnerService.ModelChangedEvent += InnerService_ModelChangedEvent;
            InnerService.CanExecuteChanged += InnerService_CanExecuteChanged;
            InnerService.ModelAddedEvent += InnerService_ModelAddedEvent;
        }

        private void InnerService_ModelAddedEvent(object sender, string e)
        {
            ModelAddedEvent?.Invoke(this, e);
        }

        private void InnerService_CanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        private void InnerService_ModelChangedEvent(object sender, EventArgs e)
        {
            ModelChangedEvent?.Invoke(this, e);
        }

        public void Save()
        {
            InnerService.Save();
        }

        public void FlushDesigner()
        {
            InnerService.FlushDesigner();
        }

        public bool CanUndo()
        {
            return InnerService.CanUndo();
        }

        public bool CanRedo()
        {
            return InnerService.CanRedo();
        }

        public bool CanCut()
        {
            return InnerService.CanCut();
        }

        public bool CanCopy()
        {
            return InnerService.CanCopy();
        }

        public bool CanPaste()
        {
            return InnerService.CanPaste();
        }

        public bool CanDelete()
        {
            return InnerService.CanDelete();
        }


        public void Undo()
        {
            InnerService.Undo();
        }

        public void Redo()
        {
            InnerService.Redo();
        }


        public void Cut()
        {
            InnerService.Cut();
        }

        public void Copy()
        {
            InnerService.Copy();
        }

        public void Paste()
        {
            InnerService.Paste();
        }

        public void Delete()
        {
            InnerService.Delete();
        }

        public void ShowCurrentLocation(string locationId)
        {
            InnerService.ShowCurrentLocation(locationId);
        }

        public void HideCurrentLocation()
        {
            InnerService.HideCurrentLocation();
        }



        public string GetActivityIdJsonArray()
        {
            return InnerService.GetActivityIdJsonArray();
        }

        public string GetBreakpointIdJsonArray()
        {
            return InnerService.GetBreakpointIdJsonArray();
        }

        public string GetTrackerVars()
        {
            return InnerService.GetTrackerVars();
        }

        public void ShowBreakpoints()
        {
            InnerService.ShowBreakpoints();
        }

        public void SetReadOnly(bool isReadOnly)
        {
            InnerService.SetReadOnly(isReadOnly);
        }

        public void RefreshArgumentsView()
        {
            InnerService.RefreshArgumentsView();
        }

        public void UpdateCurrentSelecteddDesigner()
        {
            InnerService.UpdateCurrentSelecteddDesigner();
        }

        public void InsertActivity(string name, string assemblyQualifiedName)
        {
            InnerService.InsertActivity(name,assemblyQualifiedName);
        }
    }
}
