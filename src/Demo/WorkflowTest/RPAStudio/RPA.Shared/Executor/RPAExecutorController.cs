using NamedPipeWrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Executor
{
    public class RPAExecutorController
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private static readonly TimeSpan _minMessageAge = TimeSpan.FromMilliseconds(5000);

        private ConcurrentDictionary<string, RPAExecutorStartupConfig> _executorStartupConfigs = new ConcurrentDictionary<string, RPAExecutorStartupConfig>();
        private string _rpaFlag;

        private NamedPipeServer<string> _commandMessageServer;

        private ConcurrentDictionary<string, string> _clientsDict = new ConcurrentDictionary<string, string>();

        public string _controllerIpcName { get; set; }

        public delegate void CompleteDelegate(string id, bool has_exception);
        public delegate void ExceptionDelegate(string id, string title, string msg);
        public delegate void LogDelegate(string id, string type, string msg, string msgDetails);
        public delegate void NotifyDelegate(string id, string notification, string notificationDetails);
        public delegate void SetWorkflowDebuggingPausedDelegate(string id, bool is_paused);
        public delegate void HideCurrentLocationDelegate(string id);
        public delegate void ShowCurrentLocationDelegate(string id, string location_id);
        public delegate void ShowLocalsDelegate(string id, string json);

        public event CompleteDelegate CompleteEvent;
        public event ExceptionDelegate ExceptionEvent;
        public event LogDelegate LogEvent;

        public event SetWorkflowDebuggingPausedDelegate SetWorkflowDebuggingPausedEvent;
        public event HideCurrentLocationDelegate HideCurrentLocationEvent;
        public event ShowCurrentLocationDelegate ShowCurrentLocationEvent;
        public event ShowLocalsDelegate ShowLocalsEvent;

        public static string GetRobotSharedIpcName()
        {
            return $"RPARobotServices_{Environment.MachineName}@{Environment.UserName}";
        }

        public static string GetStudioSharedIpcName()
        {
            return "RPAStudio.exe_" + Guid();
        }


        public bool InitStudio(bool isFromDebug)
        {
            _rpaFlag = "RPAStudio";
            if (_commandMessageServer != null)
            {
                _commandMessageServer.Stop();
            }

#if RPA_EXECUTOR_IPC_DEBUG
            var flag = isFromDebug ? "DEBUG" : "RUN";
            _controllerIpcName = "RPAStudio.exe_" + Guid() + $"#{flag}";
#else
            _controllerIpcName = "RPAStudio.exe_" + Guid(); ;

#endif
            _commandMessageServer = new NamedPipeServer<string>(_controllerIpcName);
            _commandMessageServer.ClientDisconnected += _commandMessageServer_ClientDisconnected;
            _commandMessageServer.ClientMessage += _commandMessageServer_ClientMessage;
            _commandMessageServer.Start();

            return true;
        }


        public bool InitRobot()
        {
            _rpaFlag = "RPARobot";
            if (_commandMessageServer != null)
            {
                _commandMessageServer.Stop();
            }

#if RPA_EXECUTOR_IPC_DEBUG
            _controllerIpcName = "RPARobotServices_TEST@TEST";
#else
            _controllerIpcName = $"RPARobotServices_{Environment.MachineName}@{Environment.UserName}";
#endif
            _commandMessageServer = new NamedPipeServer<string>(_controllerIpcName);
            _commandMessageServer.ClientDisconnected += _commandMessageServer_ClientDisconnected;
            _commandMessageServer.ClientMessage += _commandMessageServer_ClientMessage;
            _commandMessageServer.Start();

            return true;
        }


        public bool InitRobotCommandLine()
        {
            _rpaFlag = "RPACommandLine";

            if (_commandMessageServer != null)
            {
                _commandMessageServer.Stop();
            }

            _controllerIpcName = "RPACommandLine.exe_" + Guid();

            _commandMessageServer = new NamedPipeServer<string>(_controllerIpcName);
            _commandMessageServer.ClientDisconnected += _commandMessageServer_ClientDisconnected;
            _commandMessageServer.ClientMessage += _commandMessageServer_ClientMessage;
            _commandMessageServer.Start();

            return true;
        }


        private void _commandMessageServer_ClientDisconnected(NamedPipeConnection<string, string> connection)
        {
            foreach (var item in _clientsDict)
            {
                if (item.Value == connection.Name)
                {
                    string val;
                    _clientsDict.TryRemove(item.Key,out val);
                    break;
                }
            }
        }


        private void _commandMessageServer_ClientMessage(NamedPipeConnection<string, string> connection, string message)
        {
            var jobj = (JObject)JsonConvert.DeserializeObject(message);

            var guid = jobj["id"].ToString();
            switch (jobj["cmd"].ToString())
            {
                case "register":
                    if (_executorStartupConfigs.ContainsKey(guid))
                    {
                        _clientsDict[guid] = connection.Name;

                        var msg = new StartJsonMessage();
                        msg.id = guid;
                        msg.is_in_debugging_state = _executorStartupConfigs[guid].IsInDebuggingState;
                        msg.initial_debug_json_config = _executorStartupConfigs[guid].InitialDebugJsonConfig.ToString();
                        msg.initial_command_line_json_config = _executorStartupConfigs[guid].InitialCommandLineJsonConfig.ToString();
                        msg.json_params = _executorStartupConfigs[guid].JsonParams.ToString();

                        msg.name = _executorStartupConfigs[guid].Name;
                        msg.version = _executorStartupConfigs[guid].Version;
                        msg.main_xaml_path = _executorStartupConfigs[guid].MainXamlPath;
                        msg.load_assembly_from_list = _executorStartupConfigs[guid].LoadAssemblyFromList;
                        msg.assembly_resolve_dll_list = _executorStartupConfigs[guid].AssemblyResolveDllList;
                        msg.project_path = _executorStartupConfigs[guid].ProjectPath;

                        Publish(connection, msg);
                    }
                    break;
                case "exception":
                    ExceptionEvent?.Invoke(guid, jobj["title"].ToString(), jobj["msg"].ToString());
                    break;
                case "complete":
                    CompleteEvent?.Invoke(guid, (bool)jobj["has_exception"]);

                    if (_executorStartupConfigs.ContainsKey(guid))
                    {
                        var msg = new ExitJsonMessage();
                        msg.id = guid;
                        Publish(connection, msg);

#if !RPA_EXECUTOR_IPC_DEBUG
                        RPAExecutorStartupConfig cfg;
                        _executorStartupConfigs.TryRemove(guid, out cfg);
#endif
                    }
                    break;
                case "log":
                    var type = jobj["type"].ToString();
                    LogEvent?.Invoke(guid, type, jobj["msg"].ToString(), jobj["msg_details"].ToString());
                    break;
                case "set_workflow_debugging_paused":
                    SetWorkflowDebuggingPausedEvent?.Invoke(guid, (bool)jobj["is_paused"]);
                    break;
                case "show_locals":
                    ShowLocalsEvent?.Invoke(guid, jobj.ToString());
                    break;
                case "hide_current_location":
                    HideCurrentLocationEvent?.Invoke(guid);
                    break;
                case "show_current_location":
                    ShowCurrentLocationEvent?.Invoke(guid, jobj["location_id"].ToString());
                    break;
                default:
                    break;
            }
        }



        public static string Guid()
        {
#if RPA_EXECUTOR_IPC_DEBUG
            return "B9907AEB-5CD1-4AC4-B995-5CD39C46A889";
#else
            return System.Guid.NewGuid().ToString();
#endif
        }

        public void SetStartupConfig(string guid, RPAExecutorStartupConfig cfg)
        {
            _executorStartupConfigs[guid] = cfg;
        }


        public RPAExecutorStartupConfig GetStartupConfig(string guid)
        {
            return _executorStartupConfigs[guid];
        }

        private NamedPipeConnection<string, string> GetConnectionById(string id)
        {
            foreach (var conn in _commandMessageServer._connections)
            {
                if (conn.Name == _clientsDict[id])
                {
                    return conn;
                }
            }

            return null;
        }

        public void UpdateAgentConfig(string guid, string key, string val)
        {
            try
            {
                if (_executorStartupConfigs.ContainsKey(guid))
                {
                    var pipe = _executorStartupConfigs[guid].PipeName;

                    var connection = GetConnectionById(guid);
                    var msg = new UpdateAgentConfigJsonMessage();
                    msg.key = key;
                    msg.val = val;
                    Publish(connection, msg);
                }
            }
            catch (Exception)
            {


            }

        }

        public string ApplicationCurrentDirectory
        {
            get
            {
                var exeFullPath = this.GetType().Assembly.Location;
                var exeDir = System.IO.Path.GetDirectoryName(exeFullPath);
                return exeDir;
            }
        }

        public string Start(string guid)
        {
            var rpaFlag = _rpaFlag;
            var arguments = $"{guid} {rpaFlag} {_controllerIpcName}";
            using (Process process = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(ApplicationCurrentDirectory,"RPAExecutor.exe"),
                    Arguments = arguments
                }
            })
            {
                process.Start();
            }

            return arguments;
        }

        public void Stop(string guid)
        {
            try
            {
                if (_executorStartupConfigs.ContainsKey(guid))
                {
                    var connection = GetConnectionById(guid);

                    var msg = new StopJsonMessage();

                    Publish(connection, msg);
                }
            }
            catch (Exception)
            {

            }

        }

        public void Continue(string guid, string opStr)
        {
            try
            {
                if (_executorStartupConfigs.ContainsKey(guid))
                {
                    var connection = GetConnectionById(guid);

                    var msg = new ContinueJsonMessage();
                    msg.next_operate = opStr;
                    Publish(connection, msg);
                }
            }
            catch (Exception)
            {

            }

        }

        private void Publish(NamedPipeConnection<string, string> connection, object value)
        {
            connection?.PushMessage(JsonConvert.SerializeObject(value));
        }



    }
}
