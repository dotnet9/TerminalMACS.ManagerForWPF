using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

namespace WorkflowActivitiesSample
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow = new WorkflowMain();
            WorkflowInvoker.Invoke(workflow);
        }
    }
}
