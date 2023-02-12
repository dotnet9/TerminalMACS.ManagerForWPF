using RPA.Interfaces.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Interfaces.Share
{
    public class SharedVars
    {
        public static IWorkflowDesignerService CurrentWorkflowDesignerService { get; set; }
    }
}
