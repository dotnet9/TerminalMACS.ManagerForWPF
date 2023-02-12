using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Interfaces.Workflow
{
    public interface IWorkflowDesignerServiceProxy
    {
        event EventHandler ModelChangedEvent;
        event EventHandler CanExecuteChanged;
        event EventHandler<string> ModelAddedEvent;

        string XamlText { get; }

        void Init(string path);

        void UpdatePath(string path);

        FrameworkElement GetDesignerView();
        FrameworkElement GetPropertyView();
        FrameworkElement GetOutlineView();

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

        void ShowCurrentLocation(string locationId);
        void HideCurrentLocation();

        void ShowBreakpoints();


        string GetActivityIdJsonArray();
        string GetBreakpointIdJsonArray();
        string GetTrackerVars();
        void SetReadOnly(bool isReadOnly);
        void FlushDesigner();
        void RefreshArgumentsView();
        void UpdateCurrentSelecteddDesigner();

        void InsertActivity(string name, string assemblyQualifiedName);
    }
}
