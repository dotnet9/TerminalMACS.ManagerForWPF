using RPA.Interfaces.Nupkg;
using RPA.Services.Nupkg;
using RPARobot.Interfaces;
using RPARobot.Services;
using RPARobot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.ServiceRegistry
{
    public class RPARobotServiceRegistry : AppServiceRegistry
    {
        public static AppServiceRegistry Instance;

        public RPARobotServiceRegistry()
        {
            Instance = this;
        }

        public override void OnRegisterServices()
        {
            _serviceLocator.RegisterTypeSingleton<IPackageIdentityService, PackageIdentityService>();
            _serviceLocator.RegisterTypeSingleton<IPackageService, PackageService>();
            _serviceLocator.RegisterTypeSingleton<IRunManagerService, RunManagerService>();
            _serviceLocator.RegisterTypeSingleton<IRobotPathConfigService, RobotPathConfigService>();

            _serviceLocator.RegisterType<ILoadDependenciesService, LoadDependenciesService>();
        }

        public override void OnRegisterViewModels()
        {
            _serviceLocator.RegisterTypeSingleton<MainViewModel>();
            _serviceLocator.RegisterTypeSingleton<AboutViewModel>();
            _serviceLocator.RegisterTypeSingleton<StartupViewModel>();

            _serviceLocator.RegisterType<PackageItemViewModel>();
        }

        public override void OnRegisterViews()
        {

        }

    }
}
