using RPA.Interfaces.Service;
using RPA.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.ServiceRegistry
{
    public abstract class AppServiceRegistry
    {
        protected IServiceLocator _serviceLocator { get; }

        public AppServiceRegistry()
        {
            _serviceLocator = new AutofacServiceLocator();
        }

        public void RegisterServices()
        {
            OnRegisterServices();
            OnRegisterViews();
            OnRegisterViewModels();
        }
        public abstract void OnRegisterServices();

        public abstract void OnRegisterViewModels();

        public abstract void OnRegisterViews();

        public T ResolveType<T>()
        {
            return _serviceLocator.ResolveType<T>();
        }

        public T TryResolveType<T>()
        {
            return _serviceLocator.TryResolveType<T>();
        }
    }

}
