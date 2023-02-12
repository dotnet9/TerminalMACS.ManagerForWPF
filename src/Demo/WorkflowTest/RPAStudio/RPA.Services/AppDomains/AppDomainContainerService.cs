using NLog;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Project;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Interfaces.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.AppDomains
{
    public class AppDomainContainerService : IAppDomainContainerService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IServiceLocator _serviceLocator;

        private string _appDomainName = "AppDomain[" + Guid.NewGuid().ToString() + "]";
        private AppDomain _appDomain;
        private IAppDomainServiceHost _appDomainServiceHost;

        public AppDomainContainerService(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public void CreateDomain()
        {
            string baseDirectory = SharedObject.Instance.ApplicationCurrentDirectory;
            AppDomainSetup info = new AppDomainSetup
            {
                LoaderOptimization = LoaderOptimization.MultiDomain,
                ApplicationBase = baseDirectory,
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            };

            _appDomain = AppDomain.CreateDomain(_appDomainName, AppDomain.CurrentDomain.Evidence, info, AppDomain.CurrentDomain.PermissionSet
                , Array.Empty<StrongName>());
        }

        public void UnloadDomain()
        {
            try
            {
                AppDomain.Unload(_appDomain);
            }
            catch (Exception err)
            {
                _logger.Error(err);
            }

        }

        private async Task<T> Create<T>()
        {
            return await Task.Run(() =>
            {
                Type typeFromHandle = typeof(T);
                if (!typeof(MarshalByRefServiceBase).IsAssignableFrom(typeFromHandle))
                {
                    throw new InvalidOperationException(string.Format("{0} 必须继承自MarshalByRefServiceBase", typeFromHandle));
                }
                T result = (T)this._appDomain.CreateInstanceAndUnwrap(typeFromHandle.Assembly.FullName, typeFromHandle.FullName);
                return result;
            });
        }


        public void RegisterCrossDomainInstance()
        {
            _appDomainServiceHost.RegisterCrossDomainInstance(_serviceLocator.ResolveType<IProjectManagerService>());
            _appDomainServiceHost.RegisterCrossDomainInstance(_serviceLocator.ResolveType<IWorkflowStateService>());
        }

        public async Task CreateHost()
        {
            _appDomainServiceHost = await Create<AppDomainServiceHost>();

            _appDomainServiceHost.Init();
            RegisterCrossDomainInstance();

            _appDomainServiceHost.RegisterServices();
        }

        public TService GetHostService<TService>()
        {
            return _appDomainServiceHost.GetService<TService>();
        }
    }
}
