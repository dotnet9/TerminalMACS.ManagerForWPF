using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Workflow
{
    public interface IWorkflowRuntime
    {
        Activity GetRootActivity();
    }
}
