using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Workflow
{
    public class WorkflowDesignerCollectServiceProxy : MarshalByRefServiceProxyBase<IWorkflowDesignerCollectService>, IWorkflowDesignerCollectServiceProxy
    {
        public WorkflowDesignerCollectServiceProxy(IAppDomainControllerService appDomainControllerService) : base(appDomainControllerService)
        {
        }

        public void Remove(string path)
        {
            InnerService.Remove(path);
        }
    }
}
