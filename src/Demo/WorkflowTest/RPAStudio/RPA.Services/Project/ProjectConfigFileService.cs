using Newtonsoft.Json;
using RPA.Interfaces.Project;
using RPA.Shared.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Project
{
    public class ProjectConfigFileService : IProjectConfigFileService
    {
        public ProjectJsonConfig ProjectJsonConfig { get; set; }

        public string CurrentProjectJsonFilePath { get; set; }

        public ProjectConfigFileService()
        {

        }

        public bool Load(string projectConfigFilePath)
        {
            CurrentProjectJsonFilePath = projectConfigFilePath;

            ProjectJsonConfig = ProcessProjectJsonConfig();
            return true;
        }

        public bool Save()
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(ProjectJsonConfig, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(CurrentProjectJsonFilePath, output);

            return true;
        }


        /// <summary>
        /// 处理project.rpa配置
        /// </summary>
        /// <returns>project.rpa结构体对象</returns>
        private ProjectJsonConfig ProcessProjectJsonConfig()
        {
            ProjectJsonConfig = null;

            var json_str = File.ReadAllText(CurrentProjectJsonFilePath);
            try
            {
                var json_cfg = JsonConvert.DeserializeObject<ProjectJsonConfig>(json_str);

                return json_cfg;
            }
            catch (Exception)
            {
                throw;
            }


        }

    }
}
