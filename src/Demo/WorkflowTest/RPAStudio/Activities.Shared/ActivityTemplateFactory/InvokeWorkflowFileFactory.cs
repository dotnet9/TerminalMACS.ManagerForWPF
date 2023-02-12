using System;
using System.Activities;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Activities.Shared.ActivityTemplateFactory
{
    public class InvokeWorkflowFileFactory : IActivityTemplateFactory
    {
        public static readonly string AssemblyQualifiedName = typeof(InvokeWorkflowFileFactory).AssemblyQualifiedName;
        public static readonly string ActivityTypeName = "RPA.Learn.Activities.Workflow.InvokeWorkflowFileActivity,RPA.Learn.Activities";

        public static string FilePath { get; set; }

        public Activity Create(DependencyObject target)
        {
            var type = Type.GetType(ActivityTypeName);
            dynamic activity = Activator.CreateInstance(type);
            activity.SetFilePath(FilePath);

            return activity;
        }
    }
}
