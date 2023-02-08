using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingParticipantSample
{
    /// <summary>
    /// 控制台跟踪参与者类
    /// </summary>
    public class ConsoleTrackingParticipant : TrackingParticipant
    {
        private const String participantName = "控制台跟踪参与者";

        public ConsoleTrackingParticipant()
        {
            Console.WriteLine($"{participantName} 创建");
        }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            Console.WriteLine($"{participantName}触发 类型名: {record.GetType().FullName}  等级: {record.Level}, 记录号: {record.RecordNumber}");

            WorkflowInstanceRecord workflowInstanceRecord = record as WorkflowInstanceRecord;
            if (workflowInstanceRecord != null)
            {
                Console.WriteLine($"工作流实例ID：{record.InstanceId} 工作流实例状态：{workflowInstanceRecord.State}");
            }

            ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
            if (activityStateRecord != null)
            {
                Console.WriteLine($"活动组件名称：{activityStateRecord.Activity.Name} 活动组件状态：{activityStateRecord.State}");
            }

            CustomTrackingRecord customTrackingRecord = record as CustomTrackingRecord;

            if ((customTrackingRecord != null) && (customTrackingRecord.Data.Count > 0))
            {
                Console.WriteLine("自定义数据：");
                foreach (string data in customTrackingRecord.Data.Keys)
                {
                    Console.WriteLine($"\t{data} : {customTrackingRecord.Data[data]}");
                }
            }
            Console.WriteLine();

        }
    }
}
