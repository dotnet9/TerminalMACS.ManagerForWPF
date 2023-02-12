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
    public class WorkflowRunService : IWorkflowRunService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IWorkflowStateService _workflowStateService;
        private IProjectManagerService _projectManagerService;

        private string _xamlPath;

        private RPAExecutorController _controller = new RPAExecutorController();
        private string _guid;

        private List<string> _activitiesDllLoadFrom = new List<string>();
        private List<string> _dependentAssemblies = new List<string>();

        private Stopwatch _workflowExecutorStopwatch = new Stopwatch();

        public WorkflowRunService(IWorkflowStateService workflowStateService, IProjectManagerService projectManagerService)
        {
            _workflowStateService = workflowStateService;
            _projectManagerService = projectManagerService;

            _controller.ExceptionEvent += _controller_ExceptionEvent;
            _controller.CompleteEvent += _controller_CompleteEvent;
            _controller.LogEvent += _controller_LogEvent;
            _controller.InitStudio(false);
        }


        private void _controller_LogEvent(string id, string type, string msg, string msgDetails)
        {
            var t = (SharedObject.enOutputType)Enum.Parse(typeof(SharedObject.enOutputType), type);
            SharedObject.Instance.Output(t, msg, msgDetails);
        }

        private void StopwatchRestart()
        {
            _workflowExecutorStopwatch.Restart();
            SharedObject.Instance.Output(SharedObject.enOutputType.Information, string.Format("{0} 开始运行", _projectManagerService.CurrentProjectJsonConfig.name));
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

            SharedObject.Instance.Output(SharedObject.enOutputType.Information, string.Format("{0} 结束运行，耗时：{1}", _projectManagerService.CurrentProjectJsonConfig.name, elapsedTime));
        }

        private void _controller_CompleteEvent(string id, bool has_exception)
        {
            _workflowStateService.RaiseEndRunEvent();
            _workflowStateService.RunningOrDebuggingFile = "";

            StopwatchStop();

            _logger.Debug(string.Format("结束执行工作流文件 {0}", _xamlPath));
        }

        private void _controller_ExceptionEvent(string id, string title, string msg)
        {
            SharedObject.Instance.Output(SharedObject.enOutputType.Error, "运行时执行错误", msg);

            Common.InvokeAsyncOnUI(() => {
                CommonWindow.ShowMainWindowNormal();
                CommonMessageBox.ShowError(title);
            });
        }

        public void Init(string xamlPath, List<string> activitiesDllLoadFrom, List<string> dependentAssemblies)
        {
            _xamlPath = xamlPath;
            _activitiesDllLoadFrom = activitiesDllLoadFrom;
            _dependentAssemblies = dependentAssemblies;
        }

        public void Run()
        {
            if (_projectManagerService.CurrentActivitiesServiceProxy.IsXamlValid(_xamlPath))
            {
                _logger.Debug(string.Format("开始执行工作流文件 {0} ……", _xamlPath));

                _workflowStateService.RunningOrDebuggingFile = _xamlPath;
                _workflowStateService.RaiseBeginRunEvent();

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

                _controller.SetStartupConfig(guid, cfg);
                var arg = _controller.Start(guid);

                _logger.Debug($"+++++++++++++++++启动进程 RPAExecutor.exe+++++++++++++++++ 命令行参数:{arg}");
            }
            else
            {
                CommonMessageBox.ShowError("工作流校验错误，请检查参数配置");
            }
        }

        public void Stop()
        {
            _controller.Stop(_guid);
            _controller_CompleteEvent(_guid, false);
        }

    }
}
