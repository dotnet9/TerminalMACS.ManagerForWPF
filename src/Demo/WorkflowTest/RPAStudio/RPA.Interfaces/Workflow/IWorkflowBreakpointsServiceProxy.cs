using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Workflow
{
    public interface IWorkflowBreakpointsServiceProxy
    {
        void LoadBreakpoints();

        void SaveBreakpoints();

        void ShowBreakpoints(string path);

        void ToggleBreakpoint(string path);

        void RemoveAllBreakpoints(string path);
    }
}
