using NLog;
using RPA.Interfaces.Share;
using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPAExecutor.Executor
{
    public class VisualTrackingParticipant : TrackingParticipant
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private WorkflowDebuggerManager _workflowDebuggerManager;


        /// <summary>
        /// 慢速调试时的事件
        /// </summary>
        public ManualResetEvent SlowStepEvent = new ManualResetEvent(false);


        /// <summary>
        /// 记录上一次的中断时候的位置（可能是断点中断，手动中断，单步调试等位置，主要供单步步过时记录步过前的位置）
        /// </summary>
        public ActivityScheduledRecord LastDebugActivityScheduledRecord { get; set; }

        /// <summary>
        /// 主要用来记录中断时当前监视的变量信息
        /// </summary>
        public ActivityStateRecord LastActivityStateRecord { get; set; }


        /// <summary>
        /// 活动id到父亲的字典
        /// </summary>
        public Dictionary<string, string> ActivityIdParentMap = new Dictionary<string, string>();//child id => parent id

        public VisualTrackingParticipant(WorkflowDebuggerManager workflowDebuggerManager)
        {
            this._workflowDebuggerManager = workflowDebuggerManager;
        }


        /// <summary>
        /// 跟踪
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="timeout">超时时间</param>
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            OnTrackingRecordReceived(record, timeout);
        }

        /// <summary>
        /// 遇到断点
        /// </summary>
        /// <param name="child">活动信息对象</param>
        /// <returns>是否需要中断</returns>
        private bool MeetingBreakpoint(ActivityInfo child)
        {
            return _workflowDebuggerManager.IsMeetingBreakpoint(child.Id);
        }

        /// <summary>
        /// 当前活动或父活动id存在
        /// </summary>
        /// <param name="id1">id1</param>
        /// <param name="id2">id2</param>
        /// <returns>是否存在</returns>
        private bool activityCurrentOrParentIdExists(string id1, string id2)
        {
            if (id1 == id2)
            {
                return true;
            }

            if (ActivityIdParentMap.ContainsKey(id1))
            {
                return activityCurrentOrParentIdExists(ActivityIdParentMap[id1], id2);
            }

            return false;
        }

        /// <summary>
        /// 中断时的操作，比如显示位置信息，显示本地变量窗体
        /// </summary>
        /// <param name="id"></param>
        private void doWaitThings(string id)
        {
            showCurrentLocation(id);
            showLocals();
        }

        /// <summary>
        /// 执行慢速调试
        /// </summary>
        /// <param name="id">id变量</param>
        private void processSlowStep(string id)
        {
            var speed_ms = 0;
            try
            {
                speed_ms = Convert.ToInt32(_workflowDebuggerManager.GetConfig("slow_step_speed_ms"));
            }
            catch (Exception)
            {

            }

            if (speed_ms > 0)
            {
                doWaitThings(id);
                SlowStepEvent.WaitOne(speed_ms);
                SlowStepEvent.Reset();
            }
        }

        /// <summary>
        /// 执行等待
        /// </summary>
        /// <param name="id">id对象</param>
        private void processWait(string id)
        {
            bool isPaused = !SlowStepEvent.WaitOne(0);

            _workflowDebuggerManager.SetWorkflowDebuggingPaused(isPaused);

            if (isPaused)
            {
                doWaitThings(id);
            }

            SlowStepEvent.WaitOne();
            SlowStepEvent.Reset();

            _workflowDebuggerManager.SetWorkflowDebuggingPaused(false);

            hideCurrentLocation();
        }

        /// <summary>
        /// 显示本地变量信息
        /// </summary>
        private void showLocals()
        {
            //Locals监视窗口显示vars和args
            _workflowDebuggerManager.ShowLocals(LastActivityStateRecord);
        }

        /// <summary>
        /// 隐藏当前位置信息
        /// </summary>
        private void hideCurrentLocation()
        {
            var nextOperateStr = this._workflowDebuggerManager.GetConfig("next_operate");
            if (nextOperateStr == "Stop")
            {
                //停止时调用Dispatcher.Invoke会卡死，所以此处直接返回不往下走
                return;
            }

            _workflowDebuggerManager.HideCurrentLocation();
        }

        /// <summary>
        /// 显示当前位置信息
        /// </summary>
        /// <param name="id"></param>
        private void showCurrentLocation(string id)
        {
            var nextOperateStr = this._workflowDebuggerManager.GetConfig("next_operate");
            if (nextOperateStr == "Stop")
            {
                //停止时调用Dispatcher.Invoke会卡死，所以此处直接返回不往下走
                return;
            }

            _workflowDebuggerManager.ShowCurrentLocation(id);
        }

        /// <summary>
        /// 跟踪过程中会触发
        /// </summary>
        /// <param name="record">记录</param>
        /// <param name="timeout">时间</param>
        protected void OnTrackingRecordReceived(TrackingRecord record, TimeSpan timeout)
        {
            _logger.Debug("OnTrackingRecordReceived=>"+ record.ToString());

            var nextOperateStr = this._workflowDebuggerManager.GetConfig("next_operate");
            var is_log_activities = this._workflowDebuggerManager.GetConfig("is_log_activities") == "true";

            _logger.Debug("next_operate => " + nextOperateStr);

            if (record is WorkflowInstanceRecord)
            {

            }
            else if (record is ActivityScheduledRecord)
            {
                var activityScheduledRecord = record as ActivityScheduledRecord;

                if (activityScheduledRecord.Child != null && this._workflowDebuggerManager.ActivityIdContains(activityScheduledRecord.Child.Id))
                {
                    ActivityIdParentMap[activityScheduledRecord.Child.Id] = activityScheduledRecord.Activity.Id;

                    if (MeetingBreakpoint(activityScheduledRecord.Child))
                    {
                        SlowStepEvent.Reset();
                        processWait(activityScheduledRecord.Child.Id);
                        LastDebugActivityScheduledRecord = activityScheduledRecord;
                    }
                    else
                    {
                        if (nextOperateStr == "Null"
                        || nextOperateStr == "Continue"
                        )
                        {
                            processSlowStep(activityScheduledRecord.Child.Id);
                        }
                        else if (nextOperateStr == "Break")
                        {
                            processWait(activityScheduledRecord.Child.Id);
                            LastDebugActivityScheduledRecord = activityScheduledRecord;
                        }
                        else if (nextOperateStr == "StepInto")
                        {
                            processWait(activityScheduledRecord.Child.Id);
                            LastDebugActivityScheduledRecord = activityScheduledRecord;
                        }
                        else if (nextOperateStr == "StepOver")
                        {
                            if (LastDebugActivityScheduledRecord != null)
                            {
                                if (activityCurrentOrParentIdExists(activityScheduledRecord.Activity.Id, LastDebugActivityScheduledRecord.Child.Id))
                                {
                                    _workflowDebuggerManager.SetWorkflowDebuggingPaused(false);
                                }
                                else
                                {
                                    processWait(activityScheduledRecord.Child.Id);
                                    LastDebugActivityScheduledRecord = activityScheduledRecord;
                                }
                            }
                            else
                            {
                                processWait(activityScheduledRecord.Child.Id);
                                LastDebugActivityScheduledRecord = activityScheduledRecord;
                            }

                        }
                    }
                }


            }
            else if (record is ActivityStateRecord)
            {
                var activityStateRecord = record as ActivityStateRecord;

                if (activityStateRecord.State == ActivityStates.Closed
                     && (activityStateRecord.Activity.TypeName == "System.Activities.Statements.Sequence"
                         || activityStateRecord.Activity.TypeName == "System.Activities.Statements.Flowchart"
                        )
                    && (nextOperateStr == "Null"
                        || nextOperateStr == "Continue"
                        )
                    )
                {
                    processSlowStep(activityStateRecord.Activity.Id);
                }

                if (activityStateRecord.State == ActivityStates.Closed
                    && (
                    activityStateRecord.Activity.TypeName == "System.Activities.Statements.Sequence"
                    || activityStateRecord.Activity.TypeName == "System.Activities.Statements.Flowchart"
                    )
                    && (
                    nextOperateStr == "StepInto"
                    || nextOperateStr == "StepOver"
                         )
                    )
                {
                    if (!activityCurrentOrParentIdExists(activityStateRecord.Activity.Id, LastDebugActivityScheduledRecord.Child.Id))
                    {
                        //此处需要判断下
                        processWait(activityStateRecord.Activity.Id);
                    }
                }

                if (is_log_activities)
                {
                    var name = activityStateRecord.Activity.Name;

                    dynamic activityObj = new ReflectionObject(activityStateRecord.Activity);
                    var activity = activityObj.Activity;

                    if (activityStateRecord.Activity.TypeName == "System.Activities.DynamicActivity")
                    {
                        name = (activity as DynamicActivity).Name;
                    }
                    else
                    {
                        name = activity.DisplayName;
                    }

                    SharedObject.Instance.Output(SharedObject.enOutputType.Trace, string.Format("{0} {1}", name, activityStateRecord.State));
                }

                LastActivityStateRecord = activityStateRecord;
            }
        }
    }
}
