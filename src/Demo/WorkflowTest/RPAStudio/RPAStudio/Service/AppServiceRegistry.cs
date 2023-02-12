using RPA.Interfaces.App;
using RPA.Interfaces.Service;
using RPA.Services.Service;
using RPAStudio.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPAStudio.Service
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
            _serviceLocator.RegisterTypeSingleton<IApplication, RPAStudioApplication>();

            OnRegisterServices();
            OnRegisterViewModels();
        }
        public abstract void OnRegisterServices();

        public abstract void OnRegisterViewModels();

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
