using NamedPipeWrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Python.Runtime;
using System;
using System.Activities.Tracking;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPA.Shared.Executor
{
    public class RPAExecutorAgent
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private static readonly TimeSpan minMessageAge = TimeSpan.FromMilliseconds(5000);

        public string guid;

        private NamedPipeClient<string> _client;

        private string target { get; set; }

        public delegate void StartDelegate(bool is_in_debugging_state, JObject debug_json_cfg, JObject command_line_json_cfg
            , string name, string version, string mainXamlPath, List<string> loadAssemblyFromList, List<string> assemblyResolveDllList, string projectPath);
        public delegate void StopDelegate();
        public delegate void ExitDelegate();
        public delegate void UpdateAgentConfigDelegate(string id, string key, string val);
        public delegate void ContinueDelegate(string id, string next_operate);
        public delegate void S2CNotificationDelegate(string id, string notification, string notification_details);

        public event StartDelegate StartEvent;
        public event StopDelegate StopEvent;
        public event ContinueDelegate ContinueEvent;
        public event ExitDelegate ExitEvent;
        public event UpdateAgentConfigDelegate UpdateAgentConfigEvent;

        public bool Init(string rpaFlag, string controllerIpcName)
        {
            target = rpaFlag;
            _client = new NamedPipeClient<string>(controllerIpcName);
            _client.Disconnected += _client_Disconnected;

            _client.ServerMessage += _client_ServerMessage;
            _client.Start();
            _client.WaitForConnection();

            return true;
        }

        public static string GetExecutorPipeName(string guid)
        {
            return $"Executor_{guid}";
        }

        public void Register(string guid)
        {
            this.guid = guid;

            var msg = new RegisterJsonMessage();
            msg.id = guid;
            Publish(msg);
        }

        private void _client_ServerMessage(NamedPipeConnection<string, string> connection, string message)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(message);

            _logger.Debug("接收到管道服务器消息:"+ message);

            switch (jobj["cmd"].ToString())
            {
                case "start":
                    var sjm = JsonConvert.DeserializeObject<StartJsonMessage>(message);
                    var debug_json_cfg = (JObject)JsonConvert.DeserializeObject(sjm.initial_debug_json_config);
                    var json_params = (JObject)JsonConvert.DeserializeObject(sjm.json_params);
                    var command_line_json_cfg = (JObject)JsonConvert.DeserializeObject(sjm.initial_command_line_json_config);
                    StartEvent?.Invoke(sjm.is_in_debugging_state, debug_json_cfg, command_line_json_cfg, sjm.name, sjm.version
                        , sjm.main_xaml_path, sjm.load_assembly_from_list, sjm.assembly_resolve_dll_list, sjm.project_path);
                    break;
                case "stop":
                    StopEvent?.Invoke();
                    break;
                case "continue":
                    ContinueEvent?.Invoke(guid, jobj["next_operate"].ToString());
                    break;
                case "exit":
                    ExitEvent?.Invoke();
                    break;
                case "update_agent_config":
                    UpdateAgentConfigEvent?.Invoke(guid, jobj["key"].ToString(), jobj["val"].ToString());
                    break;
                default:
                    break;
            }
        }

        private void _client_Disconnected(NamedPipeConnection<string, string> connection)
        {
            Environment.Exit(0);//连接关了自动断开
        }

        public void OnExecutionCompleted(bool hasException)
        {
            var msg = new CompleteJsonMessage();
            msg.id = guid;
            msg.has_exception = hasException;
            Thread.Sleep(1000);//执行完成后等待下，因为有可能有未发送完的异步消息
            Publish(msg);
        }

        public void OnException(string title, string exceptionMsg)
        {
            var msg = new ExceptionJsonMessage();
            msg.id = guid;
            msg.title = title;
            msg.msg = exceptionMsg;
            Publish(msg);
        }

        public void RedirectLogToOutputWindow(string type, string msgStr, string msgDetails)
        {
            //该处是在UI线程里调用
            var msg = new LogJsonMessage();
            msg.id = guid;
            msg.type = type;
            msg.msg = msgStr;
            msg.msg_details = msgDetails;
            Publish(msg);
        }

        public void RedirectNotification(string notification, string notificationDetails)
        {
            var msg = new NotificationJsonMessage();
            msg.id = guid;
            msg.notification = notification;
            msg.notification_details = notificationDetails;
            Publish(msg);
        }


        public void SetWorkflowDebuggingPaused(bool isPaused)
        {
            var msg = new SetWorkflowDebuggingPausedJsonMessage();
            msg.id = guid;
            msg.is_paused = isPaused;
            Publish(msg);
        }

        private string TranslateValue(object value)
        {
            var str = "";
            try
            {
                if (value is PyObject)
                {
                    str = (value as PyObject).ToString();
                }
                else
                {
                    str = JsonConvert.SerializeObject(value);
                }

            }
            catch (Exception)
            {
                str = "无法解析";
            }

            return str;
        }

        public void ShowLocals(ActivityStateRecord activityStateRecord)
        {
            var msg = new ShowLocalsJsonMessage();
            msg.id = guid;

            foreach (var item in activityStateRecord.Variables)
            {
                msg.Variables[item.Key] = TranslateValue(item.Value);
            }

            foreach (var item in activityStateRecord.Arguments)
            {
                msg.Arguments[item.Key] = TranslateValue(item.Value);
            }

            Publish(msg);
        }

        public void HideCurrentLocation()
        {
            var msg = new HideCurrentLocationJsonMessage();
            msg.id = guid;
            Publish(msg);
        }

        public void ShowCurrentLocation(string locationId)
        {
            var msg = new ShowCurrentLocationJsonMessage();
            msg.id = guid;
            msg.location_id = locationId;
            Publish(msg);
        }


        private void Publish(object value)
        {
            _client.PushMessage(JsonConvert.SerializeObject(value));
        }

    }
}