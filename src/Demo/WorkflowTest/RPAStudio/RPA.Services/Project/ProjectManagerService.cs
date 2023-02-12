using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RPA.Interfaces.Activities;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Nupkg;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using RPA.Shared.Project;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Project
{
    public class ProjectManagerService : MarshalByRefServiceBase, IProjectManagerService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IServiceLocator _serviceLocator;

        private IAppDomainControllerService _appDomainControllerService;

        private IPackageIdentityService _packageIdentityService;

        private IPackageRepositoryService _packageRepositoryService;

        private IActivityMountService _activityMountService;

        private IWorkflowStateService _workflowStateService;
        private IProjectConfigFileService _projectConfigFileService;

        public event EventHandler ProjectLoadingBeginEvent;
        public event EventHandler ProjectLoadingEndEvent;
        public event EventHandler ProjectLoadingExceptionEvent;

        public event EventHandler ProjectPreviewOpenEvent;
        public event EventHandler ProjectOpenEvent;

        public event EventHandler<CancelEventArgs> ProjectPreviewCloseEvent;
        public event EventHandler ProjectCloseEvent;

        public ProjectJsonConfig CurrentProjectJsonConfig { get; private set; }

        public string CurrentProjectConfigFilePath { get; private set; }

        public string CurrentProjectPath { get; private set; }

        public List<string> AllActivityConfigXmls { get; private set; }

        public List<ActivityGroupOrLeafItem> Activities { get; private set; }

        public Dictionary<string, ActivityGroupOrLeafItem> ActivitiesTypeOfDict { get; private set; } = new Dictionary<string, ActivityGroupOrLeafItem>();

        public IActivitiesServiceProxy CurrentActivitiesServiceProxy { get; private set; }

        public string CurrentProjectMainXamlFileAbsolutePath
        {
            get
            {
                return CurrentProjectPath + @"\" + CurrentProjectJsonConfig.main;
            }
        }


        public List<string> CurrentActivitiesDllLoadFrom { get; private set; }

        public List<string> CurrentDependentAssemblies { get; private set; }

        public ProjectManagerService(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _packageRepositoryService = _serviceLocator.ResolveType<IPackageRepositoryService>();
            _workflowStateService = _serviceLocator.ResolveType<IWorkflowStateService>();
            _activityMountService = _serviceLocator.ResolveType<IActivityMountService>();
            _appDomainControllerService = _serviceLocator.ResolveType<IAppDomainControllerService>();
            _projectConfigFileService = _serviceLocator.ResolveType<IProjectConfigFileService>();
            _packageIdentityService = _serviceLocator.ResolveType<IPackageIdentityService>();
        }


        /// <summary>
        /// 初始化project.rpa文件
        /// </summary>
        private void InitProjectJson(string projectsPath, string projectName, string projectDescription, string projectVersion)
        {
            var config = new ProjectJsonConfig();
            config.name = projectName;
            config.description = projectDescription;
            config.main = ProjectConstantConfig.MainXamlFileName;

            if (!string.IsNullOrEmpty(projectVersion))
            {
                config.projectVersion = projectVersion;
            }

            //Packages目录下的nupkg包遍历，然后保存到依赖包对象里
            _packageRepositoryService.Init(SharedObject.Instance.ApplicationCurrentDirectory + @"\"+ ProjectConstantConfig.OfflinePackagesName);
            var list = _packageRepositoryService.GetMatchedPackagesByIdAndMaxVersion(ProjectConstantConfig.ProjectDefaultDependentPackagesMatchRegex);

            foreach (var item in list)
            {
                config.dependencies[item.Id] = $"[{item.Version}]";
            }

            CurrentProjectJsonConfig = config;

            string json = JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented);

            CurrentProjectConfigFilePath = projectsPath + @"\" + projectName + @"\" + ProjectConstantConfig.ProjectConfigFileNameWithSuffix;
            using (FileStream fs = new FileStream(CurrentProjectConfigFilePath, FileMode.Create))
            {
                //写入 
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(json);
                }
            }

        }

        private void InitMainXaml(string projectsPath, string projectName)
        {
            Common.WriteResourceContentByUri(projectsPath + @"\" + projectName + @"\" + ProjectConstantConfig.MainXamlFileName
                ,$"pack://application:,,,/RPA.Resources;Component/FileTemplates/Main.xaml");
        }


        public bool NewProject(string projectsPath, string projectName, string projectDescription, string projectVersion)
        {
            var projectDir = projectsPath + @"\" + projectName;

            CurrentProjectPath = projectDir;

            //创建项目目录
            try
            {
                Directory.CreateDirectory(projectDir);
            }
            catch (Exception e)
            {
                //创建项目失败
                _logger.Error(e);

                CommonMessageBox.ShowError("创建项目目录失败，请检查！");
                return false;
            }

            InitProjectJson(projectsPath, projectName, projectDescription, projectVersion);

            InitMainXaml(projectsPath, projectName);

            return true;
        }

        public bool SaveCurrentProjectJson()
        {
            try
            {
                string json = JsonConvert.SerializeObject(CurrentProjectJsonConfig, Newtonsoft.Json.Formatting.Indented);
                using (FileStream fs = new FileStream(CurrentProjectConfigFilePath, FileMode.Create))
                {
                    //写入 
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(json);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool CloseCurrentProject()
        {
            if (string.IsNullOrEmpty(CurrentProjectPath))
            {
                return true;
            }

            if (_workflowStateService.IsRunningOrDebugging)
            {
                CommonMessageBox.ShowInformation("请停止当前正在运行或调试的项目再关闭");
                return false;
            }

            //如果有未保存的文档，询问用户是否要关闭
            CancelEventArgs cancelEventArgs = new CancelEventArgs();
            ProjectPreviewCloseEvent?.Invoke(this, cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                return false;
            }

            //触发关闭事件
            ProjectCloseEvent?.Invoke(this, EventArgs.Empty);

            CurrentProjectPath = CurrentProjectConfigFilePath = "";
            CurrentProjectJsonConfig = null;
            ActivitiesTypeOfDict.Clear();

            //卸载设计域
            _appDomainControllerService.UnloadAppDomain();

            return true;
        }


        public async void ReopenCurrentProject()
        {
            var currentProjectConfigFilePath = CurrentProjectConfigFilePath;
            CloseCurrentProject();
            await OpenProject(currentProjectConfigFilePath);
        }

        public bool IsProjectExists(string projectConfigFilePath)
        {
            if (!File.Exists(projectConfigFilePath))
            {
                return false;
            }

            return true;
        }

        public async Task OpenProject(string projectConfigFilePath)
        {
            try
            {
                ProjectLoadingBeginEvent?.Invoke(this, EventArgs.Empty);

                await Task.Run(async () =>
                {
                    CurrentProjectConfigFilePath = projectConfigFilePath;
                    CurrentProjectPath = Path.GetDirectoryName(projectConfigFilePath);

                    _projectConfigFileService.Load(projectConfigFilePath);
                    CurrentProjectJsonConfig = _projectConfigFileService.ProjectJsonConfig;

                    await Common.InvokeAsyncOnUI(async () =>
                    {
                        await _appDomainControllerService.CreateAppDomain();

                        await Task.Run(async () =>
                        {
                            ProjectPreviewOpenEvent?.Invoke(this, EventArgs.Empty);

                            await InitDependencies();

                            CurrentActivitiesServiceProxy = _serviceLocator.ResolveType<IActivitiesServiceProxy>();

                            CurrentActivitiesServiceProxy.SetSharedObjectInstance();

                            ProjectOpenEvent?.Invoke(this, EventArgs.Empty);

                            ProjectLoadingEndEvent?.Invoke(this, EventArgs.Empty);
                        });
                    });

                });
            }
            catch (Exception e)
            {
                _logger.Error(e);
                CommonMessageBox.ShowError("打开项目时发生异常，请检查！");

                ProjectLoadingExceptionEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task InitDependencies()
        {
            await Task.Run(async () =>
            {
                var assemblies = new List<string>();
                foreach (JProperty jp in (JToken)this.CurrentProjectJsonConfig.dependencies)
                {
                    string minVersion = null;
                    if (CommonNuget.VersionRangeIsMinInclusive((string)jp.Value, out minVersion))
                    {
                        var target_ver = minVersion;

                        var retList = await _packageIdentityService.BuildDependentAssemblies(jp.Name, target_ver);
                        assemblies = assemblies.Union(retList).ToList<string>();
                    }
                }

                //动态获取代理类来执行，因为域要创建后才能调用
                var service = _serviceLocator.ResolveType<IActivitiesServiceProxy>();

                CurrentActivitiesDllLoadFrom = service.Init(assemblies);
                CurrentDependentAssemblies = assemblies;

                AllActivityConfigXmls = service.CustomActivityConfigXmls;

                var systemActivities = Common.GetResourceContentByUri($"pack://application:,,,/RPA.Resources;Component/Configs/SystemActivities.xml");
                AllActivityConfigXmls.Insert(0, systemActivities);

                List<ActivityGroupOrLeafItem> activitiesCurrent = new List<ActivityGroupOrLeafItem>();
                foreach (var activityConfigXml in AllActivityConfigXmls)
                {
                    var activitiesToMount = _activityMountService.Transform(activityConfigXml);

                    activitiesCurrent = _activityMountService.Mount(activitiesCurrent, activitiesToMount);
                }

                Activities = activitiesCurrent;

                InitActivitiesTypeOfDict(Activities);
            });
        }

        private void InitActivitiesTypeOfDict(List<ActivityGroupOrLeafItem> list)
        {
            foreach (var item in list)
            {
                if (item is ActivityGroupItem)
                {
                    var group = item as ActivityGroupItem;
                    InitActivitiesTypeOfDict(group.Children);
                }
                else
                {
                    var leaf = item as ActivityLeafItem;
                    ActivitiesTypeOfDict[leaf.TypeOf] = leaf;
                }
            }
        }

        public bool IsAlreadyOpened(string projectConfigFilePath)
        {
            return CurrentProjectConfigFilePath == projectConfigFilePath;
        }

        public void UpdateCurrentProjectConfigFilePath(string projectConfigFilePath)
        {
            CurrentProjectConfigFilePath = projectConfigFilePath;
            CurrentProjectPath = Path.GetDirectoryName(projectConfigFilePath);

            _projectConfigFileService.Load(projectConfigFilePath);
            CurrentProjectJsonConfig = _projectConfigFileService.ProjectJsonConfig;
        }

        public string GetCurrentProjectDependencyVersionById(string id)
        {
            var ret = "";

            foreach (JProperty jp in (JToken)CurrentProjectJsonConfig.dependencies)
            {
                if (jp.Name == id)
                {
                    string minVersion = null;
                    if(CommonNuget.VersionRangeIsMinInclusive((string)jp.Value,out minVersion))
                    {
                        ret = minVersion;
                    }

                    break;
                }
            }

            return ret;
        }
    }
}
