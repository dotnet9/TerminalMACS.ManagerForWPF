using System;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingParticipantSample
{
    /// <summary>
    /// 该示例演示了跟踪参与者如何使用，在工作流调试运行时会用到该技术点
    /// </summary>
    class Program
    {
        static Activity ConstructWorkflow()
        {
            return new Sequence()
            {
                Activities =
                    {
                        new WriteLine() { Text = "下面运行一个自定义的用户活动" },
                        new UserActivity(),
                        new WriteLine() { Text = "流程即将结束" },
                    }
            };
        }

        static void Main(string[] args)
        {
            const string all = "*";

            ConsoleTrackingParticipant consoleTrackingParticipant = new ConsoleTrackingParticipant()
            {
                // 创建跟踪配置文件以订阅跟踪记录
                // 当前示例只订阅工作流实例记录和活动状态记录
                TrackingProfile = new TrackingProfile()
                {
                    Name = "ConsoleTrackingProfile",
                    Queries =
                    {
                        new CustomTrackingQuery()
                        {
                         Name = all,
                         ActivityName = all
                        },
                        new WorkflowInstanceQuery()
                        {
                            //只记录启动和完成的工作流状态
                            States = { WorkflowInstanceStates.Started, WorkflowInstanceStates.Completed },
                        },
                        new ActivityStateQuery()
                        {
                            // 订阅所有活动和所有状态
                            ActivityName = all,
                            States = { all },
                        }
                    }
                }
            };


            WorkflowInvoker invoker = new WorkflowInvoker(ConstructWorkflow());
            invoker.Extensions.Add(consoleTrackingParticipant);

            invoker.Invoke();

            Console.ReadKey();

        }
    }
}
