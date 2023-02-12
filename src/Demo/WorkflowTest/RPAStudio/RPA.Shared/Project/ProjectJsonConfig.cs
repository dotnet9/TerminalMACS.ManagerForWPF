using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Project
{
    /// <summary>
    /// project.rpa的JSON文件格式
    /// </summary>
    public class ProjectJsonConfig
    {
        [JsonProperty(Order = 1)]
        public string schemaVersion { get; set; } = "1.0.0.1";//schema版本,配置文件升级时用

        [JsonProperty(Required = Required.Always, Order = 2)]
        public string studioVersion { get; set; } = Common.GetProgramVersion(); //设计器版本

        [JsonProperty(Required = Required.Always, Order = 3)]
        public string projectType { get; set; } = "Workflow";//项目类型

        [JsonProperty(Order = 4)]
        public string projectVersion { get; set; } = "1.0.0";//项目版本

        [JsonProperty(Required = Required.Always, Order = 5)]
        public string name { get; set; }//名称

        [JsonProperty(Order = 6)]
        public string description { get; set; }//描述

        [JsonProperty(Required = Required.Always, Order = 7)]
        public string main { get; set; }//主文件

        [JsonProperty(Order = 8)]
        public JObject dependencies = new JObject();//依赖项

    }
}
