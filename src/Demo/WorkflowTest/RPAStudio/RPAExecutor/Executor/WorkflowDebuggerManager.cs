using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.Workflow;
using RPA.Interfaces.Share;
using RPA.Services.Workflow;
using RPA.Shared.Executor;
using System;
using System.Activities;
using System.Activities.Tracking;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPAExecutor.Executor
{
    public class WorkflowDebuggerManager : IWorkflowManager
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private ConcurrentDictionary<string, string> _configDict = new ConcurrentDictionary<string, string>();

        private WorkflowApplication _app;
        private string _name;
        private string _version;
        private string _mainXamlPath;
        private RPAExecutorAgent _agent;


        /// <summary>
        /// 跟踪器
        /// </summary>
        private VisualTrackingParticipant _simTracker;

        public WorkflowDebuggerManager(RPAExecutorAgent agent, string name, string version, string mainXamlPath)
        {
            this._agent = agent;
            this._name = name;
            this._version = version;
            this._mainXamlPath = mainXamlPath;
        }

        public bool HasException { get; set; }//判断运行过程中是否发生了异常


        public void Continue()
        {
            _simTracker.SlowStepEvent.Set();
        }

        public void Run()
        {
            HasException = false;

            Activity workflow = ActivityXamlServices.Load(_mainXamlPath);

            try
            {
                var result = ActivityValidationServices.Validate(workflow);
                if (result.Errors.Count == 0)
                {
                    _logger.Debug(string.Format("开始调试工作流文件 {0} ……", _mainXamlPath));

                    if (_app != null)
                    {
                        _app.Terminate("");
                    }

                    _app = new WorkflowApplication(workflow);

                    _simTracker = generateTracker();
                    _app.Extensions.Add(_simTracker);

                    _app.Extensions.Add(new LogToOutputWindowTextWriter());

                    if (workflow is DynamicActivity)
                    {
                        var wr = new WorkflowRuntime();
                        wr.RootActivity = workflow;
                        _app.Extensions.Add(wr);
                    }

                    _app.OnUnhandledException = WorkflowApplicationOnUnhandledException;
                    _app.Completed = WorkflowApplicationExecutionCompleted;
                    _app.Run();
                }
                else
                {
                    _logger.Debug(string.Format("工作流校验错误，请检查参数配置： {0} ……", _mainXamlPath));
                    _agent.OnExecutionCompleted(true);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err);
                _agent.OnExecutionCompleted(true);
            }

        }

        public string GetConfig(string key)
        {
            if (_configDict.ContainsKey(key))
                return _configDict[key];
            else
                return "";
        }

        public void SetConfig(string key, string val)
        {
            _logger.Debug($"SetConfig,key={key},val={val}");
            _configDict[key] = val;
        }


        /// <summary>
        /// 停止工作流运行
        /// </summary>
        public void Stop()
        {
            if (_app != null)
            {
                try
                {
                    _app.Terminate("执行由用户主动停止", new TimeSpan(0, 0, 0, 5));
                }
                catch (Exception err)
                {
                    _logger.Error(err);
                    Environment.Exit(-1);
                }
            }
        }


        public bool IsMeetingBreakpoint(string id)
        {
            var jarr_str = GetConfig("breakpoint_id_json_array");
            if (jarr_str != "")
            {
                JArray jarr = JArray.Parse(jarr_str);

                foreach (JToken val in jarr)
                {
                    if ((string)val == id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetWorkflowDebuggingPaused(bool isPaused)
        {
            _agent.SetWorkflowDebuggingPaused(isPaused);
        }

        public void ShowLocals(ActivityStateRecord activityStateRecord)
        {
            _agent.ShowLocals(activityStateRecord);
        }

        public void HideCurrentLocation()
        {
            _agent.HideCurrentLocation();
        }

        public void ShowCurrentLocation(string id)
        {
            _agent.ShowCurrentLocation(id);
        }

        public bool ActivityIdContains(string id)
        {
            var jarr_str = GetConfig("activity_id_json_array");
            if (jarr_str != "")
            {
                JArray jarr = JArray.Parse(jarr_str);
                foreach (JToken val in jarr)
                {
                    if ((string)val == id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="e">参数</param>
        /// <returns>异常对象</returns>
        private UnhandledExceptionAction WorkflowApplicationOnUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs e)
        {
            HasException = true;

            var name = e.ExceptionSource.DisplayName;
            SharedObject.Instance.Output(SharedObject.enOutputType.Error, string.Format("{0} 执行时出现异常", name), e.UnhandledException.ToString());

            return UnhandledExceptionAction.Terminate;
        }

        /// <summary>
        /// 工作流执行完成
        /// </summary>
        /// <param name="obj">对象</param>
        private void WorkflowApplicationExecutionCompleted(WorkflowApplicationCompletedEventArgs obj)
        {
            if (obj.TerminationException != null)
            {
                if (!string.IsNullOrEmpty(obj.TerminationException.Message))
                {
                    HasException = true;

                    _agent.OnException(obj.TerminationException.Message, obj.TerminationException.ToString());
                }
            }

            _agent.OnExecutionCompleted(HasException);

            _logger.Debug(string.Format("结束调试工作流文件 {0}", _mainXamlPath));
        }

        public void RedirectLogToOutputWindow(SharedObject.enOutputType type, string msg, string msgDetails)
        {
            _agent.RedirectLogToOutputWindow(type.ToString(), msg, msgDetails);
        }

        public void RedirectNotification(string notification, string notificationDetails)
        {
            _agent.RedirectNotification(notification, notificationDetails);
        }

        /// <summary>
        /// 生成跟踪器
        /// </summary>
        /// <returns>生成的跟踪器</returns>
        private VisualTrackingParticipant generateTracker()
        {
            const String all = "*";

            VisualTrackingParticipant simTracker = new VisualTrackingParticipant(this)
            {
                TrackingProfile = new TrackingProfile()
                {
                    Name = "CustomTrackingProfile",
                    Queries =
                        {
                         new CustomTrackingQuery()
                            {
                                Name = all,
                                ActivityName = all
                            },
                            new WorkflowInstanceQuery()
                            {
                                States = { WorkflowInstanceStates.Started, WorkflowInstanceStates.Completed },
                            },

                             new ActivityStateQuery()
                            {
                                ActivityName = all,
                                States = { all },

                                Variables =
                                {
                                     all
                                },

                                Arguments =
                                {
                                    all
                                },
                            },

                             new ActivityScheduledQuery()
                            {
                                ActivityName = all,
                                ChildActivityName = all
                            },
                        }
                }
            };

            trackerVarsAdd(simTracker);

            return simTracker;
        }

        private void trackerVarsAdd(VisualTrackingParticipant simTracker)
        {
            Collection<string> variables = null;

            foreach (var item in simTracker.TrackingProfile.Queries)
            {
                if (item is ActivityStateQuery)
                {
                    var query = item as ActivityStateQuery;

                    variables = query.Variables;
                    break;
                }
            }

            var jarr_str = GetConfig("tracker_vars");
            if (jarr_str != "")
            {
                JArray jarr = JArray.Parse(jarr_str);

                foreach (JToken val in jarr)
                {
                    variables.Add((string)val);
                }
            }

        }

        public void OnUnhandledException(string title, Exception err)
        {
            _agent.OnException(title, err.ToString());
            _agent.OnExecutionCompleted(true);
        }


    }
}
