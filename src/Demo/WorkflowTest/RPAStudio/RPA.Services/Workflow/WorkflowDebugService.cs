using Newtonsoft.Json;
using NLog;
using RPA.Interfaces.Project;
using RPA.Interfaces.Workflow;
using RPA.Interfaces.Share;
using RPA.Shared.Executor;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Workflow
{
    public class WorkflowDebugService : IWorkflowDebugService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IWorkflowStateService _workflowStateService;
        private IProjectManagerService _projectManagerService;

        private string _xamlPath;

        private RPAExecutorController _controller = new RPAExecutorController();
        private string _guid;

        private List<string> _activitiesDllLoadFrom = new List<string>();
        private List<string> _dependentAssemblies = new List<string>();


        private IWorkflowDesignerServiceProxy _workflowDesignerServiceProxy;

        private string _activityIdJsonArray { get; set; }
        private string _breakpointIdJsonArray { get; set; }

        private Stopwatch _workflowExecutorStopwatch = new Stopwatch();

        public WorkflowDebugService(IWorkflowStateService workflowStateService,IProjectManagerService projectManagerService)
        {
            _workflowStateService = workflowStateService;
            _projectManagerService = projectManagerService;

            _workflowStateService.UpdateSlowStepSpeedEvent += _workflowStateService_UpdateSlowStepSpeedEvent;
            _workflowStateService.UpdateIsLogActivitiesEvent += _workflowStateService_UpdateIsLogActivitiesEvent;
            _workflowStateService.BreakpointsModifyEvent += _workflowStateService_BreakpointsModifyEvent;

            _controller.ExceptionEvent += _controller_ExceptionEvent;
            _controller.CompleteEvent += _controller_CompleteEvent;
            _controller.LogEvent += _controller_LogEvent;
            _controller.SetWorkflowDebuggingPausedEvent += _controller_SetWorkflowDebuggingPausedEvent;
            _controller.HideCurrentLocationEvent += _controller_HideCurrentLocationEvent;
            _controller.ShowCurrentLocationEvent += _controller_ShowCurrentLocationEvent;
            _controller.ShowLocalsEvent += _controller_ShowLocalsEvent;
            _controller.InitStudio(true);
        }



        private void _workflowStateService_BreakpointsModifyEvent(object sender, EventArgs e)
        {
            _breakpointIdJsonArray = _workflowDesignerServiceProxy.GetBreakpointIdJsonArray();
            _controller.UpdateAgentConfig(_guid, "breakpoint_id_json_array", _breakpointIdJsonArray.ToString());
        }

        private void _workflowStateService_UpdateIsLogActivitiesEvent(object sender, bool e)
        {
            _workflowStateService.IsLogActivities = e;
            _controller.UpdateAgentConfig(_guid, "is_log_activities", e ? "true" : "false");
        }

        private void _workflowStateService_UpdateSlowStepSpeedEvent(object sender, enSpeed e)
        {
            _workflowStateService.SpeedType = e;

            _workflowStateService.SpeedMS = GetSpeed(e);

            _controller.UpdateAgentConfig(_guid, "slow_step_speed_ms", _workflowStateService.SpeedMS.ToString());
        }


        public void SetSpeed(enSpeed speedType)
        {
            _workflowStateService.SpeedMS = GetSpeed(speedType);
        }

        private int GetSpeed(enSpeed speedType)
        {
            int speed_ms = 0;
            switch (speedType)
            {
                case enSpeed.Off:
                    speed_ms = 0;
                    break;
                case enSpeed.One:
                    speed_ms = 2000;
                    break;
                case enSpeed.Two:
                    speed_ms = 1000;
                    break;
                case enSpeed.Three:
                    speed_ms = 500;
                    break;
                case enSpeed.Four:
                    speed_ms = 250;
                    break;
                default:
                    speed_ms = 0;
                    break;
            }

            return speed_ms;
        }

        private void _controller_LogEvent(string id, string type, string msg, string msgDetails)
        {
            var t = (SharedObject.enOutputType)Enum.Parse(typeof(SharedObject.enOutputType), type);
            SharedObject.Instance.Output(t, msg, msgDetails);
        }


        private void _controller_CompleteEvent(string id, bool has_exception)
        {
            _workflowStateService.RaiseEndDebugEvent();
            _workflowStateService.RunningOrDebuggingFile = "";

            StopwatchStop();

            _logger.Debug(string.Format("结束调试工作流文件 {0}", _xamlPath));
        }

        private void _controller_ExceptionEvent(string id, string title, string msg)
        {
            SharedObject.Instance.Output(SharedObject.enOutputType.Error, "运行时执行错误", msg);

            CommonWindow.ShowMainWindowNormal();

            Common.InvokeAsyncOnUI(() =>
            {
                CommonMessageBox.ShowError(title);
            });
        }

        private void _controller_ShowLocalsEvent(string id, string json)
        {
            var showLocalsJsonMessage = JsonConvert.DeserializeObject<ShowLocalsJsonMessage>(json);

            _workflowStateService.RaiseShowLocalsEvent(showLocalsJsonMessage);
        }

        private void _controller_ShowCurrentLocationEvent(string id, string location_id)
        {
            if (!_workflowStateService.IsRunningOrDebugging)
            {
                return;
            }

            _workflowDesignerServiceProxy.ShowCurrentLocation(location_id);
        }

        private void _controller_HideCurrentLocationEvent(string id)
        {
            _workflowDesignerServiceProxy.HideCurrentLocation();
        }

        private void _controller_SetWorkflowDebuggingPausedEvent(string id, bool is_paused)
        {
            _workflowStateService.IsDebuggingPaused = is_paused;
        }

        public void Init(IWorkflowDesignerServiceProxy workflowDesignerServiceProxy, string xamlPath
            , List<string> activitiesDllLoadFrom, List<string> dependentAssemblies)
        {
            _workflowDesignerServiceProxy = workflowDesignerServiceProxy;
            _xamlPath = xamlPath;
            _activitiesDllLoadFrom = activitiesDllLoadFrom;
            _dependentAssemblies = dependentAssemblies;
        }

        public void Break()
        {
            SetNextOperate(enOperate.Break);
        }

        public void Continue(enOperate operate = enOperate.Continue)
        {
            SetNextOperate(operate);
            _controller.Continue(_guid, operate.ToString());
        }


        private void StopwatchRestart()
        {
            _workflowExecutorStopwatch.Restart();
            SharedObject.Instance.Output(SharedObject.enOutputType.Information, string.Format("{0} 开始调试", _projectManagerService.CurrentProjectJsonConfig.name));
        }

        private void StopwatchStop()
        {
            _workflowExecutorStopwatch.Stop();

            string elapsedTime = "";
            var elapsedTimeSpan = _workflowExecutorStopwatch.Elapsed.Duration();
            if (_workflowExecutorStopwatch.Elapsed.Days > 0)
            {
                elapsedTime = elapsedTimeSpan.ToString("%d") + " day(s) " + elapsedTimeSpan.ToString(@"hh\:mm\:ss");
            }
            else
            {
                elapsedTime = elapsedTimeSpan.ToString(@"hh\:mm\:ss");
            }

            SharedObject.Instance.Output(SharedObject.enOutputType.Information, string.Format("{0} 结束调试，耗时：{1}", _projectManagerService.CurrentProjectJsonConfig.name, elapsedTime));
        }

        public void Debug()
        {
            if (_projectManagerService.CurrentActivitiesServiceProxy.IsXamlValid(_xamlPath))
            {
                _logger.Debug(string.Format("开始调试工作流文件 {0} ……", _xamlPath));

                InitDebugInfo();

                _workflowStateService.RunningOrDebuggingFile = _xamlPath;

                _workflowStateService.RaiseBeginDebugEvent();

                StopwatchRestart();

                var guid = RPAExecutorController.Guid();

                _guid = guid;

                var cfg = new RPAExecutorStartupConfig();
                cfg.Name = _projectManagerService.CurrentProjectJsonConfig.name;
                cfg.Version = _projectManagerService.CurrentProjectJsonConfig.projectVersion;
                cfg.MainXamlPath = _xamlPath;
                cfg.PipeName = RPAExecutorAgent.GetExecutorPipeName(guid);
                cfg.LoadAssemblyFromList = _activitiesDllLoadFrom;
                cfg.AssemblyResolveDllList = _dependentAssemblies;
                cfg.ProjectPath = SharedObject.Instance.ProjectPath;

                cfg.IsInDebuggingState = true;
                //调试初始化配置
                cfg.InitialDebugJsonConfig["next_operate"] = _workflowStateService.NextOperate.ToString();
                cfg.InitialDebugJsonConfig["slow_step_speed_ms"] = _workflowStateService.SpeedMS.ToString();
                cfg.InitialDebugJsonConfig["is_log_activities"] = _workflowStateService.IsLogActivities ? "true" : "false";
                cfg.InitialDebugJsonConfig["activity_id_json_array"] = _activityIdJsonArray;
                cfg.InitialDebugJsonConfig["breakpoint_id_json_array"] = _breakpointIdJsonArray;
                cfg.InitialDebugJsonConfig["tracker_vars"] = _workflowDesignerServiceProxy.GetTrackerVars();

                _controller.SetStartupConfig(guid, cfg);
                var arg = _controller.Start(guid);

                _logger.Debug($"+++++++++++++++++启动进程 RPAExecutor.exe+++++++++++++++++ 命令行参数:{arg}");
            }
            else
            {
                CommonMessageBox.ShowError("工作流校验错误，请检查参数配置");
            }
        }

        private void InitDebugInfo()
        {
            _activityIdJsonArray = _workflowDesignerServiceProxy.GetActivityIdJsonArray();
            _breakpointIdJsonArray = _workflowDesignerServiceProxy.GetBreakpointIdJsonArray();

            _controller.UpdateAgentConfig(_guid, "activity_id_json_array", _activityIdJsonArray);
            _controller.UpdateAgentConfig(_guid, "breakpoint_id_json_array", _breakpointIdJsonArray.ToString());
        }

        public void SetNextOperate(enOperate operate)
        {
            _workflowStateService.NextOperate = operate;

            if (!string.IsNullOrEmpty(_guid))
            {
                _controller.UpdateAgentConfig(_guid, "next_operate", operate.ToString());
            }
        }

        public void Stop()
        {
            SetNextOperate(enOperate.Null);
            _controller_HideCurrentLocationEvent("");
            _controller.Stop(_guid);
            _controller_CompleteEvent(_guid, false);
        }


    }
}
