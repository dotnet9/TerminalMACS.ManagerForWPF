using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Workflow
{
    public interface IWorkflowStateService
    {
        event EventHandler BeginRunEvent;
        event EventHandler EndRunEvent;

        event EventHandler BeginDebugEvent;
        event EventHandler EndDebugEvent;

        event EventHandler BreakpointsModifyEvent;

        event EventHandler<object> ShowLocalsEvent;

        event EventHandler<enSpeed> UpdateSlowStepSpeedEvent;

        event EventHandler<bool> UpdateIsLogActivitiesEvent;


        string RunningOrDebuggingFile { get; set; }

        bool IsRunning { get; set; }

        bool IsDebugging { get; set; }

        bool IsRunningOrDebugging { get; }

        bool IsDebuggingPaused { get; set; }

        bool IsLogActivities { get; set; }

        enSpeed SpeedType { get; set; }

        int SpeedMS { get; set; }

        enOperate NextOperate { get; set; }

        void RaiseBeginRunEvent();

        void RaiseEndRunEvent();

        void RaiseBeginDebugEvent();

        void RaiseEndDebugEvent();

        void RaiseBreakpointsModifyEvent();

        void RaiseShowLocalsEvent(object msg);

        void RaiseUpdateSlowStepSpeedEvent(enSpeed speed);

        void RaiseUpdateIsLogActivitiesEvent(bool isLogActivities);


    }
}
