using ReflectionMagic;
using System;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Activities.Presentation.ViewState;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Debugger
{
    public static class RPADebugger
    {
        public static ModelService GetModelService(WorkflowDesigner workflowDesigner)
        {
            if (workflowDesigner == null)
            {
                return null;
            }
            return workflowDesigner.Context.Services.GetService<ModelService>();
        }

        public static ModelItem GetRootModelItem(WorkflowDesigner workflowDesigner)
        {
            ModelService modelService = GetModelService(workflowDesigner);
            if (modelService == null)
            {
                return null;
            }
            return modelService.Root;
        }

        public static Activity GetRootWorkflowElement(WorkflowDesigner workflowDesigner)
        {
            ModelItem rootModelItem = GetRootModelItem(workflowDesigner);
            object obj = (rootModelItem != null) ? rootModelItem.GetCurrentValue() : null;
            IDebuggableWorkflowTree debuggableWorkflowTree;
            if (obj == null || (debuggableWorkflowTree = (obj as IDebuggableWorkflowTree)) == null)
            {
                return obj as Activity;
            }
            return debuggableWorkflowTree.GetWorkflowRoot();
        }


        public static void InitDictionary<T1, T2>(ref Dictionary<T1, T2> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<T1, T2>();
                return;
            }
            dictionary.Clear();
        }

        public static string GetIdRef(Activity activity)
        {
            string text = WorkflowViewState.GetIdRef(activity);
            if (text == null)
            {
                text = string.Empty;
            }
            return text;
        }

        public static void BuildSourceLocationMappings(Dictionary<object, SourceLocation> wfElementToSourceLocationMapping, ref Dictionary<string, SourceLocation> activityIdToSourceLocationMapping)
        {
            InitDictionary<string, SourceLocation>(ref activityIdToSourceLocationMapping);
            using (Dictionary<object, SourceLocation>.KeyCollection.Enumerator enumerator = wfElementToSourceLocationMapping.Keys.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Activity activity;
                    if ((activity = (enumerator.Current as Activity)) != null && !string.IsNullOrEmpty(activity.Id))
                    {
                        if (!activityIdToSourceLocationMapping.ContainsKey(activity.Id))
                        {
                            activityIdToSourceLocationMapping.Add(activity.Id, wfElementToSourceLocationMapping[activity]);
                        }
                        string idRef = GetIdRef(activity);
                        if (!string.IsNullOrEmpty(idRef) && !activityIdToSourceLocationMapping.ContainsKey(idRef))
                        {
                            activityIdToSourceLocationMapping.Add(idRef, wfElementToSourceLocationMapping[activity]);
                        }
                    }
                }
            }
        }


        public static void BuildSourceLocationMappings(WorkflowDesigner workflowDesigner, ref Dictionary<string, SourceLocation> activityIdToSourceLocationMapping)
        {
            workflowDesigner.Flush();

            dynamic dmv = workflowDesigner.DebugManagerView.AsDynamic();
            dmv.InvalidateSourceLocationMapping(workflowDesigner.Context.Items.GetValue<WorkflowFileItem>().LoadedFile);
            dmv.EnsureSourceLocationUpdated();

            Dictionary<object, SourceLocation> dictionary = new Dictionary<object, SourceLocation>();

            dictionary = dmv.instanceToSourceLocationMapping;
            dmv.UpdateSourceLocations(dictionary);

            BuildSourceLocationMappings(dictionary, ref activityIdToSourceLocationMapping);

        }


    }
}
