using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Workflow
{
    public class WorkflowStateService : MarshalByRefServiceBase, IWorkflowStateService
    {
        public event EventHandler BeginRunEvent;

        public event EventHandler EndRunEvent;

        public event EventHandler BeginDebugEvent;

        public event EventHandler EndDebugEvent;
        public event EventHandler BreakpointsModifyEvent;
        public event EventHandler<object> ShowLocalsEvent;
        public event EventHandler<enSpeed> UpdateSlowStepSpeedEvent;
        public event EventHandler<bool> UpdateIsLogActivitiesEvent;

        public string RunningOrDebuggingFile { get; set; }

        public bool IsDebugging { get; set; }

        public bool IsDebuggingPaused { get; set; }

        public bool IsRunning { get; set; }

        public bool IsRunningOrDebugging
        {
            get
            {
                return IsRunning || IsDebugging;
            }
        }

        public bool IsLogActivities { get; set; }

        public enOperate NextOperate { get; set; }


        private int _speedMS = 0;

        public int SpeedMS
        {
            get
            {
                return _speedMS;
            }

            set
            {
                _speedMS = value;
            }
        }

        private enSpeed _speedType = enSpeed.Off;
        public enSpeed SpeedType
        {
            get
            {
                return _speedType;
            }

            set
            {
                _speedType = value;
                switch (_speedType)
                {
                    case enSpeed.Off:
                        _speedMS = 0;
                        break;
                    case enSpeed.One:
                        _speedMS = 2000;
                        break;
                    case enSpeed.Two:
                        _speedMS = 1000;
                        break;
                    case enSpeed.Three:
                        _speedMS = 500;
                        break;
                    case enSpeed.Four:
                        _speedMS = 250;
                        break;
                    default:
                        _speedMS = 0;
                        break;
                }
            }
        }



        public void RaiseBeginRunEvent()
        {
            IsRunning = true;
            BeginRunEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseEndRunEvent()
        {
            IsRunning = false;
            EndRunEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseBeginDebugEvent()
        {
            IsDebugging = true;
            BeginDebugEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseEndDebugEvent()
        {
            IsDebugging = false;
            EndDebugEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseBreakpointsModifyEvent()
        {
            BreakpointsModifyEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseShowLocalsEvent(object msg)
        {
            ShowLocalsEvent?.Invoke(this, msg);
        }

        public void RaiseUpdateSlowStepSpeedEvent(enSpeed speed)
        {
            UpdateSlowStepSpeedEvent?.Invoke(this, speed);
        }

        public void RaiseUpdateIsLogActivitiesEvent(bool isLogActivities)
        {
            UpdateIsLogActivitiesEvent?.Invoke(this, isLogActivities);
        }
    }
}
