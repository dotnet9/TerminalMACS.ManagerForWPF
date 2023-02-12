using RPA.Interfaces.Workflow;
using RPA.Interfaces.Share;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Workflow
{
    /// <summary>
    /// 工作流运行时类
    /// </summary>
    public class WorkflowRuntime : IWorkflowRuntime
    {
        /// <summary>
        /// 根活动
        /// </summary>
        public Activity RootActivity { get; set; }

        /// <summary>
        /// 获取根活动
        /// </summary>
        /// <returns>根活动</returns>
        public Activity GetRootActivity()
        {
            return RootActivity;
        }
    }
}
