using RPA.Interfaces.Nupkg;
using RPA.Services.Nupkg;
using RPACommandLine.Interfaces;
using RPACommandLine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPACommandLine.ServiceRegistry
{
    public class RPACommandLineServiceRegistry : AppServiceRegistry
    {
        public static AppServiceRegistry Instance;

        public RPACommandLineServiceRegistry()
        {
            Instance = this;
        }

        public override void OnRegisterServices()
        {
            _serviceLocator.RegisterTypeSingleton<IGlobalService, GlobalService>();
            _serviceLocator.RegisterTypeSingleton<IRunManagerService, RunManagerService>();
            _serviceLocator.RegisterTypeSingleton<IProjectService, ProjectService>();
            _serviceLocator.RegisterTypeSingleton<IPackageIdentityService, PackageIdentityService>();
            _serviceLocator.RegisterType<ILoadDependenciesService, LoadDependenciesService>();
        }

        public override void OnRegisterViewModels()
        {

        }

        public override void OnRegisterViews()
        {

        }
    }
}
