using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Executor
{
    public class RegisterJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "register";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }
    }


    public class StartJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "start";

        [JsonProperty(Required = Required.Always)]
        public bool is_in_debugging_state { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string initial_debug_json_config { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string initial_command_line_json_config { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string version { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string main_xaml_path { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<string> load_assembly_from_list;

        [JsonProperty(Required = Required.Always)]
        public List<string> assembly_resolve_dll_list;

        [JsonProperty(Required = Required.Always)]
        public string project_path;

        [JsonProperty(Required = Required.Always)]
        public string json_params;
    }

    public class StopJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "stop";
    }

    public class ContinueJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "continue";

        [JsonProperty(Required = Required.Always)]
        public string next_operate { get; set; }
    }

    public class CompleteJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "complete";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool has_exception;
    }

    public class ExceptionJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "exception";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string title { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string msg { get; set; }
    }

    public class ExitJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "exit";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }
    }

    public class LogJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "log";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string type { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string msg { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string msg_details { get; set; }
    }


    public class NotificationJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "notification";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string notification { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string notification_details { get; set; }
    }

    public class S2CNotifyJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "s2c_notification";

        [JsonProperty(Required = Required.Always)]
        public string notification { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string notification_details { get; set; }
    }



    public class UpdateAgentConfigJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "update_agent_config";

        [JsonProperty(Required = Required.Always)]
        public string key { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string val { get; set; }
    }


    public class SetWorkflowDebuggingPausedJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "set_workflow_debugging_paused";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool is_paused { get; set; }
    }

    public class ShowLocalsJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "show_locals";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();

        [JsonProperty(Required = Required.Always)]
        public Dictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
    }


    public class HideCurrentLocationJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "hide_current_location";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }
    }


    public class ShowCurrentLocationJsonMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string cmd { get; set; } = "show_current_location";

        [JsonProperty(Required = Required.Always)]
        public string id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string location_id { get; set; }
    }






}
