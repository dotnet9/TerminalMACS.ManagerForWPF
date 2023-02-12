using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.Nupkg;
using RPA.Shared.Project;
using RPA.Shared.Utils;
using RPACommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPACommandLine.Services
{
    public class LoadDependenciesService : ILoadDependenciesService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IPackageIdentityService _packageIdentityService;

        /// <summary>
        /// 当前项目project.rpa文件路径
        /// </summary>
        public string CurrentProjectJsonFile { get; set; }

        public List<string> CurrentActivitiesDllLoadFrom { get; private set; }
        public List<string> CurrentDependentAssemblies { get; private set; }

        public LoadDependenciesService(IPackageIdentityService packageIdentityService)
        {
            _packageIdentityService = packageIdentityService;
        }

        public void Init(string projectJsonFile)
        {
            CurrentProjectJsonFile = projectJsonFile;
        }

        /// <summary>
        /// 加载项目依赖包
        /// </summary>
        public async Task LoadDependencies()
        {
            await Task.Run(async () =>
            {
                var json_cfg = ReadProjectJsonConfig();

                var assemblies = new List<string>();
                foreach (JProperty jp in (JToken)json_cfg.dependencies)
                {
                    string minVersion = null;
                    if (CommonNuget.VersionRangeIsMinInclusive((string)jp.Value, out minVersion))
                    {
                        var target_ver = minVersion;

                        var retList = await _packageIdentityService.BuildDependentAssemblies(jp.Name, target_ver);
                        assemblies = assemblies.Union(retList).ToList<string>();
                    }
                }

                CurrentActivitiesDllLoadFrom = assemblies;
                CurrentDependentAssemblies = assemblies;
            });

        }

        /// <summary>
        /// 处理project.rpa文件
        /// </summary>
        /// <returns>json对象结构</returns>
        public ProjectJsonConfig ReadProjectJsonConfig()
        {
            var json_str = File.ReadAllText(CurrentProjectJsonFile);
            try
            {
                var json_cfg = JsonConvert.DeserializeObject<ProjectJsonConfig>(json_str);
                return json_cfg;
            }
            catch (Exception err)
            {
                throw err;
            }


        }

    }
}
