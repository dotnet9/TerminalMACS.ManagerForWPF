using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.Service;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPACommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPACommandLine.Services
{
    public class ProjectService : IProjectService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public string ProjectDirectory { get; set; }

        public string ProjectJsonFile { get; set; }

        public string XamlFile { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        private IServiceLocator _serviceLocator;
        private IRunManagerService _runManagerService;
        private ILoadDependenciesService _loadDependenciesService;

        public ProjectService(IServiceLocator serviceLocator, IRunManagerService runManagerService,ILoadDependenciesService loadDependenciesService)
        {
            _serviceLocator = serviceLocator;

            _runManagerService = runManagerService;
            _loadDependenciesService = loadDependenciesService;
        }

        public void Init(string filePath)
        {
            ProjectDirectory = System.IO.Path.GetDirectoryName(filePath);

            string fileName = System.IO.Path.GetFileName(filePath);
            if (fileName.ToLower() == ProjectConstantConfig.ProjectConfigFileNameWithSuffix)
            {
                ProjectJsonFile = filePath;

                if (System.IO.File.Exists(ProjectJsonFile))
                {
                    //找到主XAML文件
                    string json = System.IO.File.ReadAllText(ProjectJsonFile);
                    JObject jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
                    var relativeMainXaml = jsonObj["main"].ToString();
                    var absoluteMainXaml = System.IO.Path.Combine(ProjectDirectory, relativeMainXaml);

                    XamlFile = absoluteMainXaml;

                    Name = jsonObj["name"].ToString();
                    Version = jsonObj["projectVersion"].ToString();
                }
            }
            else
            {
                ProjectJsonFile = ProjectDirectory + @"\"+ ProjectConstantConfig.ProjectConfigFileNameWithSuffix;

                XamlFile = filePath;

                if (System.IO.File.Exists(ProjectJsonFile))
                {
                    string json = System.IO.File.ReadAllText(ProjectJsonFile);
                    JObject jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);

                    Name = jsonObj["name"].ToString();
                    Version = jsonObj["projectVersion"].ToString();
                }
            }
        }


        private void RunWorkflow(List<string> activitiesDllLoadFrom, List<string> dependentAssemblies)
        {
            System.GC.Collect();//提醒系统回收内存，避免内存占用过高

            SharedObject.Instance.ProjectPath = ProjectDirectory;
            SharedObject.Instance.OutputEvent += Instance_OutputEvent;

            _runManagerService.Init(Name, Version, XamlFile, activitiesDllLoadFrom, dependentAssemblies);
            _runManagerService.Run();
        }

        private void Instance_OutputEvent(SharedObject.enOutputType type, string msg, string msgDetails = "")
        {
            _logger.Debug(string.Format("活动日志：type={0},msg={1},msgDetails={2}", type, msg, msgDetails));
        }

        public void Start()
        {
            if (System.IO.File.Exists(ProjectJsonFile))
            {
                //项目配置文件存在
                Task.Run(async () =>
                {
                    try
                    {
                        var serv = _serviceLocator.ResolveType<ILoadDependenciesService>();
                        serv.Init(ProjectJsonFile);
                        await serv.LoadDependencies();

                        RunWorkflow(serv.CurrentActivitiesDllLoadFrom, serv.CurrentDependentAssemblies);
                    }
                    catch (Exception err)
                    {
                        _logger.Error(err);
                    }
                });
            }
        }


        public void Stop()
        {
            _runManagerService.Stop();
        }

    }
}
