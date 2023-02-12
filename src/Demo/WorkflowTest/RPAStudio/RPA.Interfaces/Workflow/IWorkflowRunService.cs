using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Workflow
{
    public interface IWorkflowRunService
    {
        void Init(string xamlPath, List<string> activitiesDllLoadFrom, List<string> dependentAssemblies);

        void Run();
        void Stop();
    }
}
