using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Workflow
{
    public class WorkflowBreakpointsServiceProxy : MarshalByRefServiceProxyBase<IWorkflowBreakpointsService>, IWorkflowBreakpointsServiceProxy
    {
        private IWorkflowStateService _workflowStateService;

        public WorkflowBreakpointsServiceProxy(IAppDomainControllerService appDomainControllerService, IWorkflowStateService workflowStateService) : base(appDomainControllerService)
        {
            _workflowStateService = workflowStateService;
        }

        public void ToggleBreakpoint(string path)
        {
            InnerService.ToggleBreakpoint(path);
            _workflowStateService.RaiseBreakpointsModifyEvent();
        }

        public void RemoveAllBreakpoints(string path)
        {
            InnerService.RemoveAllBreakpoints(path);

            _workflowStateService.RaiseBreakpointsModifyEvent();
        }

        public void ShowBreakpoints(string path)
        {
            InnerService.ShowBreakpoints(path);
        }

        public void LoadBreakpoints()
        {
            InnerService.LoadBreakpoints();
        }

        public void SaveBreakpoints()
        {
            InnerService.SaveBreakpoints();
        }
    }
}
