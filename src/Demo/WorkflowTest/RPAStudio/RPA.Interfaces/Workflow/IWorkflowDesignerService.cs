using System;
using System.Activities.Presentation;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Workflow
{
    public interface IWorkflowDesignerService
    {
        event EventHandler ModelChangedEvent;
        event EventHandler CanExecuteChanged;
        event EventHandler<string> ModelAddedEvent;

        string Path { get; }

        string XamlText { get; }

        void Init(string path);

        void UpdatePath(string path);

        INativeHandleContract GetDesignerView();
        INativeHandleContract GetPropertyView();
        INativeHandleContract GetOutlineView();

        void Save();

        bool CanUndo();
        bool CanRedo();
        bool CanCut();
        bool CanCopy();
        bool CanPaste();
        bool CanDelete();


        void Undo();
        void Redo();
        void Cut();
        void Copy();
        void Paste();
        void Delete();

        WorkflowDesigner GetWorkflowDesigner();

        void ShowCurrentLocation(string locationId);
        void HideCurrentLocation();

        string GetActivityIdJsonArray();
        string GetBreakpointIdJsonArray();
        string GetTrackerVars();

        void ShowBreakpoints();
        void SetReadOnly(bool isReadOnly);
        void FlushDesigner();
        void RefreshArgumentsView();
        void UpdateCurrentSelecteddDesigner();

        void InsertActivity(string name, string assemblyQualifiedName);
    }
}
